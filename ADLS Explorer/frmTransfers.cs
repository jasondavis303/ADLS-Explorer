using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ADLS_Explorer
{
    public partial class frmTransfers : Form
    {
        readonly AZContainer _container;
        readonly bool _isUpload = false;
        readonly List<TransferInfo> _transferInfos = new List<TransferInfo>();
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        bool _hasCancelled = false;

        public frmTransfers(AZContainer container, IEnumerable<TransferInfo> transfers, bool isUpload)
        {
            InitializeComponent();
            _container = container;
            _transferInfos.AddRange(transfers);
            _isUpload = isUpload;

            if (_isUpload)
                _transferInfos.Sort((x, y) => x.LocalObject.CompareTo(y.LocalObject));
            else
                _transferInfos.Sort((x, y) => x.CloudObject.Name.CompareTo(y.CloudObject.Name));
        }               

        private async void frmTransfers_Load(object sender, EventArgs e)
        {
            Show();

            int curIndex = 0;

            IProgress<double> fileProgress = new Progress<double>(d =>
            {
                int val = Math.Min(Math.Max((int)d, 0), 100);
                pbFile.Value = val;
                pbFile.Refresh();
                pbTotal.Value = (curIndex * 100) + val;
                pbTotal.Refresh();
            });


            try
            {
                OverwriteMode mode = OverwriteMode.Unknown;

                pbTotal.Maximum = _transferInfos.Count * 100;


                for (int i = 0; i < _transferInfos.Count; i++)
                {
                    if (_hasCancelled)
                        break;


                    curIndex = i;
                    fileProgress.Report(0);

                    var cloudFile = _transferInfos[i].CloudObject;
                    var localFile = _transferInfos[i].LocalObject;

                    if (_isUpload)
                    {
                        lblFile.Text = NativeMethods.CompactPath(localFile, 100);
                        lblFile.Refresh();

                        bool doUpload = true;
                        
                        if (mode != OverwriteMode.YesAll)
                        {
                            if (_transferInfos[i].CloudObjectExists)
                            {
                                if (mode == OverwriteMode.Unknown)
                                {
                                    var f = new frmOverwrite(cloudFile.Name);
                                    if (f.ShowDialog() != DialogResult.OK)
                                        throw new Exception("Operation cancelled");
                                    if (f.SelectedMode == OverwriteMode.YesAll)
                                        mode = OverwriteMode.YesAll;
                                    if (f.SelectedMode == OverwriteMode.NoAll)
                                        mode = OverwriteMode.NoAll;

                                    if (f.SelectedMode == OverwriteMode.No || f.SelectedMode == OverwriteMode.NoAll)
                                        doUpload = false;
                                }
                                else if (mode == OverwriteMode.NoAll)
                                {
                                    doUpload = false;
                                }
                            }
                        }

                        if (doUpload)
                            await AZService.UploadFileAsync(_container, cloudFile, localFile, fileProgress, _cancellationTokenSource.Token);
                    }
                    else
                    {
                        lblFile.Text = NativeMethods.CompactPath(cloudFile.Name, 100);
                        lblFile.Refresh();

                        Directory.CreateDirectory(Path.GetDirectoryName(localFile));

                        bool doDownload = true;
                        if (File.Exists(localFile))
                        {
                            if (mode == OverwriteMode.Unknown)
                            {
                                var f = new frmOverwrite(localFile);
                                if (f.ShowDialog() != DialogResult.OK)
                                    throw new Exception("Operation cancelled");

                                if (f.SelectedMode == OverwriteMode.YesAll)
                                    mode = OverwriteMode.YesAll;
                                if (f.SelectedMode == OverwriteMode.NoAll)
                                    mode = OverwriteMode.NoAll;

                                if (f.SelectedMode == OverwriteMode.No || f.SelectedMode == OverwriteMode.NoAll)
                                    doDownload = false;
                            }
                            else if (mode == OverwriteMode.NoAll)
                            {
                                doDownload = false;
                            }

                            if (doDownload)
                                File.Delete(localFile);
                        }
                        
                        if(doDownload)
                            await AZService.DownloadFileAsync(_container, cloudFile, localFile, fileProgress, _cancellationTokenSource.Token);
                    }

                    fileProgress.Report(100);
                }
            }
            catch (Exception ex)
            {
                ex.Show();
            }

            _hasCancelled = true;
            Close();
        }


        private void lblFile_Paint(object sender, PaintEventArgs e)
        {
            /*
             Dim fn As String = IO.Path.GetFileName(Label1.Text)
        Dim pth As String = IO.Path.GetFullPath(Label1.Text)
        With e.Graphics
            .Clear(Me.BackColor)
            Dim sw As Single = .MeasureString(Label1.Text, Label1.Font).Width
            If sw > Label1.Width Then
                pth &= "...\" & fn
                While sw > Label1.Width
                    pth = pth.Remove(pth.IndexOf("...\") - 1, 1)
                    sw = .MeasureString(pth, Label1.Font).Width
                End While
            End If
            Using sf As New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center, .FormatFlags = StringFormatFlags.NoWrap}
                .DrawString(pth, Label1.Font, Brushes.Black, New Rectangle(0, 0, Label1.Width, Label1.Height), sf)
            End Using
        End With 
            */
            
        }

        private void btnCancel_Click(object sender, EventArgs e) => ConfirmCancel();

        private void frmTransfers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_hasCancelled)
            {
                e.Cancel = true;
                ConfirmCancel();
            }
        }

        private void ConfirmCancel()
        {
            var ans = MessageBox.Show("Are you sure you want to cancel transfers in progress?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ans != DialogResult.Yes)
                return;

            _hasCancelled = true;
            _cancellationTokenSource.Cancel();
        }

       
    }
}
