using System;
using System.Windows.Forms;

namespace ADLS_Explorer
{
    public enum OverwriteMode
    {
        Unknown = 0,
        Yes = 1,
        YesAll = 2,
        No = 3,
        NoAll = 4
    }


    public partial class frmOverwrite : Form
    {
        readonly string _path;

        public frmOverwrite(string path)
        {
            InitializeComponent();
            _path = path;
            lblFile.Text = $"'{path}'";
        }

        private void frmOverwrite_Load(object sender, EventArgs e)
        {
            Height = (Height - ClientSize.Height) + tableLayoutPanel1.Height;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            SelectedMode = OverwriteMode.Yes;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnYesall_Click(object sender, EventArgs e)
        {
            SelectedMode = OverwriteMode.YesAll;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            SelectedMode = OverwriteMode.No;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnNoAll_Click(object sender, EventArgs e)
        {
            SelectedMode = OverwriteMode.NoAll;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public OverwriteMode SelectedMode { get; private set; }
    }
}
