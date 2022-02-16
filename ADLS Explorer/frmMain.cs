using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADLS_Explorer
{
    public partial class frmMain : Form
    {
        Settings _settings;
        AZFSO _root = new AZFSO();

        public frmMain()
        {
            InitializeComponent();

            Icon = Properties.Resources.blue_cloud_storage;

            ilIcons.Images.Add(Constants.CLOSED_FOLDER_ICON, NativeMethods.GetClosedFolderIcon());
            ilIcons.Images.Add(Constants.OPEN_FOLDER_ICON, NativeMethods.GetOpenFolderIcon());

            btnSettings.Image = FontAwesome.Type.Cog.AsImage();

            mnuNewFolder.Image = FontAwesome.Type.Folder.AsImage(Color.DarkOrange);
            mnuRefreshFiles.Image = FontAwesome.Type.Refresh.AsImage(Color.Green);
            mnuUpload.Image = FontAwesome.Type.CloudUpload.AsImage(Color.Blue);
            mnuDownload.Image = FontAwesome.Type.Download.AsImage(Color.Blue);
            mnuMove.Image = FontAwesome.Type.FolderOpen.AsImage(Color.DarkOrange);
            mnuRename.Image = FontAwesome.Type.Font.AsImage();
            mnuDelete.Image = FontAwesome.Type.Remove.AsImage(Color.Red);

            mnuRefreshFolders.Image = FontAwesome.Type.Refresh.AsImage(Color.Green);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Show();
            _settings = Settings.Load();
            if(_settings.AZContainers.Count == 0)
            {
                new frmSettings().ShowDialog();
                _settings = Settings.Load();
                if(_settings.AZContainers.Count == 0)
                {
                    new Exception("You must specify at least 1 container to use this app!").Show();
                    Application.Exit();
                    return;
                }
            }

            ReadSettings();
        }

        private void cbCurrentContainer_SelectedIndexChanged(object sender, EventArgs e)
        {            
            lvFiles.Items.Clear();
            tvFolders.Nodes.Clear();           
            if (cbCurrentContainer.SelectedIndex < 0)
                return;
            
            EnableControls(false);

            try
            {
                _settings.MostRecent = CurrentAZContainer.ToString();
                _settings.Save();

                _root = new AZFSO();
                var rootNode = tvFolders.Nodes.Add("ROOT", "ROOT", Constants.CLOSED_FOLDER_ICON, Constants.OPEN_FOLDER_ICON);
                rootNode.Tag = _root;
                tvFolders.SelectedNode = rootNode;
            }
            catch (Exception ex)
            {
                ex.Show();
                EnableControls(true);
            }

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
            _settings = Settings.Load();
            if (_settings.AZContainers.Count == 0)
            {
                new Exception("You must specify at least 1 container to use this app!").Show();
                Application.Exit();
                return;
            }


            ReadSettings();
        }

        private async void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lvFiles.Items.Clear();
            if (e.Node == null)
                return;

            EnableControls(false);

            var parent = e.Node.GetFSO();


            if (!parent.ChildrenLoaded)
            {
                try
                {
                    await e.Node.AddTreeNodes(CurrentAZContainer, parent);
                }
                catch (Exception ex)
                {
                    ex.Show();
                }
            }


            foreach (var child in parent.Children)
            {
                if (!child.IsDirectory)
                    if (!ilIcons.Images.ContainsKey(child.Extension))
                        ilIcons.Images.Add(child.Extension, NativeMethods.GetFileIcon(child.Extension));

                string icon = child.IsDirectory ? Constants.CLOSED_FOLDER_ICON : child.Extension;

                var lvi = lvFiles.Items.Add(child.Filename, icon);
                lvi.SubItems.Add(child.FileType);
                lvi.SubItems.Add(child.SizeString);
                lvi.SubItems.Add(child.LastModified.ToString("MM/dd/yyyy hh:mm:ss tt"));
                lvi.SubItems.Add(child.Permissions);
                lvi.SubItems.Add(child.Owner);
                lvi.Tag = child;
            }

            EnableControls(true);
        }

        private void mnuRefreshFolders_Click(object sender, EventArgs e)
        {
            RefreshDirectory();
        }

        private async void mnuNewFolder_Click(object sender, EventArgs e)
        {
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Name:", "New Folder");
            if (string.IsNullOrEmpty(newName))
                return;

            EnableControls(false);

            try
            {
                await AZService.CreateDirectoryAsync(CurrentAZContainer, tvFolders.SelectedNode.GetFSO(), newName);
            }
            catch (Exception ex)
            {
                ex.Show();
            }

            tvFolders.SelectedNode.GetFSO().ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));
        }

        private void lvFiles_DoubleClick(object sender, EventArgs e)
        {
            if (tvFolders.SelectedNode == null)
                return;

            if (lvFiles.SelectedItems.Count == 0)
                return;
            
            if (lvFiles.SelectedItems.Count > 1)
                return;

            var lvi = lvFiles.SelectedItems[0];
            var azFSO = lvi.GetFSO();
            if(azFSO.IsDirectory)
            {
                tvFolders.SelectedNode = tvFolders.Nodes.Find(azFSO.Name, true).FirstOrDefault();
            }
            else
            {
                //File
                throw new NotImplementedException();
            }
        }


        private async void lvFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null || files.Length == 0)
                return;
            await Upload(files, tvFolders.SelectedNode.GetFSO());
        }

        private void lvFiles_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void lvFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var obj = new DataObject();
          
        }

        private void cmsFiles_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(tvFolders.SelectedNode == null)
            {
                e.Cancel = true;
                return;
            }

            int cnt = lvFiles.SelectedItems.Count;            

            mnuRename.Enabled = cnt == 1;
            mnuMove.Enabled = cnt > 0;
            mnuDelete.Enabled = cnt > 0;
            mnuDownload.Enabled = cnt > 0;
        }

        private void mnuRefreshFiles_Click(object sender, EventArgs e)
        {
            RefreshDirectory();
        }

        private async void mnuUploadFiles_Click(object sender, EventArgs e)
        {
            if (dlgFiles.ShowDialog() != DialogResult.OK)
                return;

            await Upload(dlgFiles.FileNames, tvFolders.SelectedNode.GetFSO());
        }

        private async void mnuUploadFolder_Click_1(object sender, EventArgs e)
        {
            if (dlgLocalFolder.ShowDialog() != DialogResult.OK)
                return;


            await Upload(new string[] { dlgLocalFolder.SelectedPath }, tvFolders.SelectedNode.GetFSO());
        }

        private async void mnuDownload_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count == 0)
                return;

            if (dlgLocalFolder.ShowDialog() != DialogResult.OK)
                return;

            IProgress<string> scanProgress = new Progress<string>(s => lblStatus.Text = "Scanning: " + s);

            EnableControls(false);

            try
            {
                var scanQueue = new Queue<TransferInfo>();
                var toDownload = new List<TransferInfo>();
                foreach (ListViewItem lvi in lvFiles.SelectedItems)
                {
                    var fso = lvi.GetFSO();
                    if (fso.IsDirectory)
                        scanQueue.Enqueue(new TransferInfo(fso, dlgLocalFolder.SelectedPath));
                    else
                        toDownload.Add(new TransferInfo(fso, Path.Combine(dlgLocalFolder.SelectedPath, fso.Filename)));
                }

                while (scanQueue.Count > 0)
                {
                    var scanItem = scanQueue.Dequeue();
                    lblStatus.Text = $"Scanning: {scanItem.CloudObject.Name}";
                    
                    var children = await AZService.GetChildObjectsAsync(CurrentAZContainer, scanItem.CloudObject, true);
                    foreach (var child in children.Where(item => !item.IsDirectory))
                        toDownload.Add(new TransferInfo
                        {
                            CloudObject = child,
                            LocalObject = Path.Combine(scanItem.LocalObject, child.Name.Substring(Path.GetDirectoryName(scanItem.CloudObject.Name).Length + 1))
                        });
                    
                }

                new frmTransfers(CurrentAZContainer, toDownload, false).ShowDialog();
            }
            catch (Exception ex)
            {
                ex.Show();
            }

            EnableControls(true);
        }

        private async void mnuMove_Click(object sender, EventArgs e)
        {
            var f = new AZFolderBrowserDialog(CurrentAZContainer, tvFolders);
            if (f.ShowDialog() != DialogResult.OK)
                return;

            EnableControls(false);

            try
            {
                foreach(ListViewItem lvi in lvFiles.SelectedItems)
                {
                    lblStatus.Text = "Moving: " + lvi.Text;
                    await AZService.MoveAsync(CurrentAZContainer, lvi.GetFSO(), f.SelectedFolder.Name);
                }
            }
            catch(Exception ex)
            {
                ex.Show();
            }

            lblStatus.Text = null;
            tvFolders.SelectedNode.GetFSO().ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));
        }

        private async void mnuRename_Click(object sender, EventArgs e)
        {
            var parentFSO = tvFolders.SelectedNode.GetFSO();
            var childFSO = lvFiles.SelectedItems[0].GetFSO();

            string title = childFSO.IsDirectory ? "Directory" : "File";
            string newName = Microsoft.VisualBasic.Interaction.InputBox("New name:", $"Rename {title}", childFSO.Filename);
            if (string.IsNullOrWhiteSpace(newName))
                return;

            string newPath = $"{parentFSO.Name}/{newName}";

            EnableControls(false);
            try
            {
                await AZService.MoveAsync(CurrentAZContainer, childFSO, newPath);
            }
            catch (Exception ex)
            {
                ex.Show();
            }

            parentFSO.ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));

        }

        private async void mnuDelete_Click(object sender, EventArgs e)
        {
            var ans = MessageBox.Show("Are you sure you want to delete the selected items?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ans != DialogResult.Yes)
                return;

            EnableControls(false);

            try
            {
                foreach (ListViewItem lvi in lvFiles.SelectedItems)
                {
                    lblStatus.Text = "Deleting: " + lvi.Text;
                    await AZService.DeleteAsync(CurrentAZContainer, lvi.GetFSO());
                }
            }
            catch (Exception ex)
            {
                ex.Show();
            }

            lblStatus.Text = null;
            tvFolders.SelectedNode.GetFSO().ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));
        }











        
        
        
        
        private void EnableControls(bool e)
        {
            UseWaitCursor = !e;
            splitContainer1.Enabled = e;
            toolStrip1.Enabled = e;
        }

        private AZContainer CurrentAZContainer => cbCurrentContainer.SelectedItem as AZContainer;

        private void ReadSettings()
        {
            var curItem = CurrentAZContainer;

            cbCurrentContainer.Items.Clear();
            _settings.AZContainers.Sort();
            _settings.AZContainers.ForEach(item => cbCurrentContainer.Items.Add(item));

            if (curItem != null)
                if (cbCurrentContainer.Items.Contains(curItem))
                    cbCurrentContainer.SelectedItem = curItem;

            if (cbCurrentContainer.SelectedIndex < 0)
            {
                if (cbCurrentContainer.Items.Count == 1)
                    cbCurrentContainer.SelectedIndex = 0;
                else
                    try { cbCurrentContainer.SelectedItem = _settings.AZContainers.FirstOrDefault(item => item.ToString() == _settings.MostRecent); }
                    catch { }
            }   
        }

        private void RefreshDirectory()
        {
            tvFolders.SelectedNode.GetFSO().ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));
        }

        private async Task Upload(IEnumerable<string> sources, AZFSO target)
        {
            var scanQueue = new Queue<TransferInfo>();
            var toUpload = new List<TransferInfo>();

            EnableControls(false);

            lblStatus.Text = $"Scanning: {target.Name}";
            var existingFiles = string.IsNullOrWhiteSpace(target.Name) ?
                await AZService.GetRootObjectsAsync(CurrentAZContainer, true) :
                await AZService.GetChildObjectsAsync(CurrentAZContainer, target, true);


            foreach(var source in sources)
            {
                lblStatus.Text = $"Scanning: {source}";
                statusStrip1.Refresh();

                try
                {
                    if ((File.GetAttributes(source) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        scanQueue.Enqueue(new TransferInfo
                        {
                            LocalObject = source,
                            CloudObject = new AZFSO { Name = $"{target.Name}/{Path.GetFileName(source)}" }
                        });
                    }
                    else
                    {
                        string targetName = $"{target.Name}/{Path.GetFileName(source)}";
                        toUpload.Add(new TransferInfo
                        {
                            CloudObject = new AZFSO
                            {
                                IsDirectory = true,
                                Name = targetName
                            },
                            CloudObjectExists = existingFiles.Any(item => item.Name == targetName),
                            LocalObject = source
                        });
                    }
                }
                catch (Exception ex)
                {
                    ex.Show();
                }
            }

            while(scanQueue.Count > 0)
            {
                var usi = scanQueue.Dequeue();
                var dirInfo = new DirectoryInfo(usi.LocalObject);
                try
                {
                    foreach (var dir in dirInfo.EnumerateDirectories())
                    {
                        lblStatus.Text = $"Scanning: {dir.FullName}";
                        statusStrip1.Refresh();

                        scanQueue.Enqueue(new TransferInfo
                        {
                            LocalObject = dir.FullName,
                            CloudObject = new AZFSO { Name = $"{usi.CloudObject.Name}/{dir.Name}" }
                        });
                    }

                    foreach (var file in dirInfo.EnumerateFiles())
                    {
                        lblStatus.Text = $"Scanning: {file.FullName}";
                        string targetName = $"{usi.CloudObject.Name}/{file.Name}";
                        toUpload.Add(new TransferInfo
                        {
                            CloudObject = new AZFSO { Name = targetName },
                            CloudObjectExists = existingFiles.Any(item => item.Name == targetName),
                            LocalObject = file.FullName
                        });
                    }
                }
                catch (Exception ex)
                {
                    ex.Show();
                }
            }

            lblStatus.Text = null;
            new frmTransfers(CurrentAZContainer, toUpload, true).ShowDialog(this);

            tvFolders.SelectedNode.GetFSO().ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));
        }

        
    }
}