using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADLS_Explorer
{
    internal static class Extensions
    {
        public static void Show(this Exception ex) => MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static AZFSO GetFSO(this TreeNode tn) => tn.Tag as AZFSO;

        public static AZFSO GetFSO(this ListViewItem lvi) => lvi.Tag as AZFSO;

        public static async Task AddTreeNodes(this TreeNode tn, AZContainer azContainer, AZFSO azFSO)
        {
            var children = tn.Parent == null ?
                        await AZService.GetRootObjectsAsync(azContainer) :
                        await AZService.GetChildObjectsAsync(azContainer, azFSO);
            azFSO.Children.Clear();
            azFSO.Children.AddRange(children);
            azFSO.ChildrenLoaded = true; 
            
            tn.Nodes.Clear();
            foreach (var child in azFSO.Children.Where(item => item.IsDirectory))
                tn.Nodes.Add(child.Name, child.Filename, Constants.CLOSED_FOLDER_ICON, Constants.OPEN_FOLDER_ICON).Tag = child;
            tn.Expand();
        }

    }
}
