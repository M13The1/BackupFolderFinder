using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BackupFolderFinder.Services;
using BackupFolderFinder.Models;
using System.Collections.Generic;


namespace BackupFolderFinder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the Backup folder";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtRootFolder.Text = dialog.SelectedPath;
                    lblStatus.Text = "Folder selected.";
                }
            }
        }

        private void DisplayRetentionResults(List<BackupRetentionGroup> groups)
        {
            dgvResults.DataSource = null;
            dgvResults.DataSource = groups;

            clmn_Date.DefaultCellStyle.Format = "dd-MMM-yyyy";

            lblStatus.Text = groups.Count + " days require cleanup.";
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRootFolder.Text))
            {
                MessageBox.Show("Please select a Backup folder.");
                return;
            }

            BackupScanner scanner = new BackupScanner();
            var backups = scanner.Scan(txtRootFolder.Text);

            BackupRetentionAnalyzer analyzer = new BackupRetentionAnalyzer();
            var retentionGroups = analyzer.Analyze(backups);

            DisplayRetentionResults(retentionGroups);
        }

        private void dgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // We'll implement this later.
        }

        private void dgvResults_SelectionChanged(object sender, EventArgs e)
        {
            dgvDeleteList.Rows.Clear();

            if (dgvResults.CurrentRow == null)
                return;

            BackupRetentionGroup group =
                dgvResults.CurrentRow.DataBoundItem as BackupRetentionGroup;

            if (group == null)
                return;

            foreach (BackupInfo backup in group.DeleteBackups)
            {
                dgvDeleteList.Rows.Add(
                    backup.Time,
                    backup.BackupFolder);
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (dgvResults.CurrentRow == null)
                return;

            BackupRetentionGroup group =
                dgvResults.CurrentRow.DataBoundItem as BackupRetentionGroup;

            if (group == null)
                return;

            DialogResult result = MessageBox.Show(
                "Delete " + group.DeleteCount +
                " duplicate backup(s)?\n\n" +
                "The first backup (" + group.KeepTime + ") will be retained.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            BackupDeleter deleter = new BackupDeleter();

            deleter.Delete(group);

            MessageBox.Show(
                "Duplicate backups deleted successfully.",
                "Completed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            btnScan.PerformClick();
        }
    }
}
