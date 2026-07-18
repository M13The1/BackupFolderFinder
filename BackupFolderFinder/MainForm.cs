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

        private void DisplayResults(List<DuplicateBackupGroup> duplicates)
        {
            dgvResults.DataSource = null;
            dgvResults.DataSource = duplicates;

            dgvResults.Columns["BackupDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";

            dgvResults.Columns["ProjectName"].HeaderText = "Project";
            dgvResults.Columns["BackupDate"].HeaderText = "Date";
            dgvResults.Columns["Count"].HeaderText = "No. of Backups";
            dgvResults.Columns["Times"].HeaderText = "Backup Times";
            dgvResults.Columns["FolderPath"].Visible = false;

            lblStatus.Text = duplicates.Count + " duplicate days found.";
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

        private void DisplayRetentionResults(List<BackupRetentionGroup> groups)
        {
            dgvResults.DataSource = null;
            dgvResults.AutoGenerateColumns = true;
            dgvResults.DataSource = groups;

            // Hide object properties
            dgvResults.Columns["KeepBackup"].Visible = false;
            dgvResults.Columns["DeleteBackups"].Visible = false;

            // Rename headers
            dgvResults.Columns["ProjectName"].HeaderText = "Project";
            dgvResults.Columns["BackupDate"].HeaderText = "Date";
            dgvResults.Columns["KeepTime"].HeaderText = "Keep";
            dgvResults.Columns["DeleteTimes"].HeaderText = "Delete";
            dgvResults.Columns["DeleteCount"].HeaderText = "Delete Count";

            // Format date
            dgvResults.Columns["BackupDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";

            lblStatus.Text = groups.Count + " days require cleanup.";
        }

        private void dgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DuplicateBackupGroup item =
                (DuplicateBackupGroup)dgvResults.Rows[e.RowIndex].DataBoundItem;

            System.Diagnostics.Process.Start("explorer.exe", item.FolderPath);
        }
    }
}
