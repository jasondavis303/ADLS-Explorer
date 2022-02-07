using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ADLS_Explorer
{
    public partial class frmSettings : Form
    {
        readonly Settings _settings;

        public frmSettings()
        {
            InitializeComponent();
            btnAdd.Image = FontAwesome.Type.Plus.AsImage(Color.Green);
            mnuDelete.Image = FontAwesome.Type.Remove.AsImage(Color.Red);

            _settings = Settings.Load();
            LoadAccountContainers();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string acct = Microsoft.VisualBasic.Interaction.InputBox("Storage Account:", "Add Container");
                if (string.IsNullOrWhiteSpace(acct))
                    return;
                acct = acct.Trim().ToLower();

                string container = Microsoft.VisualBasic.Interaction.InputBox("Container:", "Add Container");
                if (string.IsNullOrWhiteSpace(container))
                    return;
                container = container.Trim().ToLower();

                if (_settings.AZContainers.Any(item => item.Account == acct && item.Container == container))
                    throw new Exception("The specified account and container already exist");

                UseWaitCursor = true;
                tableLayoutPanel1.Enabled = false;
                btnAdd.Enabled = false;

                await AZService.GetRootObjectsAsync(new AZContainer { Account = acct, Container = container });
                
                //OK to add and save
                _settings.AZContainers.Add(new AZContainer { Account = acct, Container = container });
                _settings.Save();
                LoadAccountContainers();
            }
            catch (Exception ex)
            {
                ex.Show();
            }

            UseWaitCursor = false;
            tableLayoutPanel1.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void cmsMain_Opening(object sender, CancelEventArgs e)
        {
            mnuDelete.Enabled = lvMain.SelectedItems.Count > 0;
        }

        
        private void mnuDelete_Click(object sender, EventArgs e)
        {
            var ret = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret != DialogResult.Yes)
                return;

            _settings.AZContainers.Remove((AZContainer)lvMain.SelectedItems[0].Tag);
            _settings.Save();
            LoadAccountContainers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void LoadAccountContainers()
        {
            lvMain.Items.Clear();
            _settings.AZContainers.Sort();
            foreach(var azContainer in _settings.AZContainers)
            {
                var lvi = lvMain.Items.Add(azContainer.Account);
                lvi.SubItems.Add(azContainer.Container);
                lvi.Tag = azContainer;
            }
        }

        
    }
}
