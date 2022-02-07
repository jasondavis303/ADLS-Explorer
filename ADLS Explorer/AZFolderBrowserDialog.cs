using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ADLS_Explorer
{
    public partial class AZFolderBrowserDialog : Form
    {
        readonly bool _initLoad;
        readonly AZContainer _container;

        public AZFolderBrowserDialog(AZContainer container, TreeView srcTree)
        {
            InitializeComponent();
            ilIcons.Images.Add(Constants.CLOSED_FOLDER_ICON, NativeMethods.GetClosedFolderIcon());
            ilIcons.Images.Add(Constants.OPEN_FOLDER_ICON, NativeMethods.GetOpenFolderIcon());
            mnuRefresh.Image = FontAwesome.Type.Refresh.AsImage(Color.Green);
            mnuCreateFolder.Image = FontAwesome.Type.Folder.AsImage(Color.DarkOrange);

            _container = container;
            _initLoad = true;

            var srcRoot = srcTree.Nodes[0];
            var thisRoot = tvFolders.Nodes.Add("ROOT", "ROOT", Constants.CLOSED_FOLDER_ICON, Constants.OPEN_FOLDER_ICON);
            thisRoot.Tag = srcRoot.Tag;
            tvFolders.SelectedNode = thisRoot;

            CopySrcTree(srcRoot, thisRoot);
            tvFolders.SelectedNode = tvFolders.Nodes.Find(srcTree.SelectedNode.GetFSO().Name, true).FirstOrDefault();

            _initLoad = false;
        }

        private async void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_initLoad)
                return;

            var parent = tvFolders.SelectedNode.GetFSO();
            if (parent.ChildrenLoaded)
                return;

            EnableControls(false);

            if (!parent.ChildrenLoaded)
            {
                try
                {                    
                    await e.Node.AddTreeNodes(_container, parent);
                }
                catch (Exception ex)
                {
                    ex.Show();
                }
            }

            EnableControls(true);
        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            tvFolders.SelectedNode.GetFSO().ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));
        }

        private async void mnuCreateFolder_Click(object sender, EventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Folder name:", "Create Folder");
            if (string.IsNullOrWhiteSpace(name))
                return;

            EnableControls(false);

            var parent = tvFolders.SelectedNode.GetFSO();

            try
            {
                await AZService.CreateDirectoryAsync(_container, parent, name);
            }
            catch (Exception ex)
            {
                ex.Show();
            }

            parent.ChildrenLoaded = false;
            tvFolders_AfterSelect(tvFolders, new TreeViewEventArgs(tvFolders.SelectedNode));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }


        private void EnableControls(bool e)
        {
            UseWaitCursor = !e;
            tableLayoutPanel1.Enabled = e;
        }


        public AZFSO SelectedFolder => tvFolders.SelectedNode.GetFSO();

        private void CopySrcTree(TreeNode srcParent, TreeNode thisParent)
        {
            foreach(TreeNode srcChild in srcParent.Nodes)
            {
                var thisChild = thisParent.Nodes.Add(srcChild.GetFSO().Name, srcChild.Text, Constants.CLOSED_FOLDER_ICON, Constants.OPEN_FOLDER_ICON);
                thisChild.Tag = srcChild.Tag;
                CopySrcTree(srcChild, thisChild);
            }
        }

        
    }
}
