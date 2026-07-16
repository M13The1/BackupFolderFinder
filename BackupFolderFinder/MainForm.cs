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

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRootFolder.Text))
            {
                MessageBox.Show(
                    "Please select a Backup folder.",
                    "Backup Folder Finder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!Directory.Exists(txtRootFolder.Text))
            {
                MessageBox.Show(
                    "The selected folder does not exist.",
                    "Backup Folder Finder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            lblStatus.Text = "Ready to scan.";

            BackupScanner scanner = new BackupScanner();

            var backups = scanner.Scan(txtRootFolder.Text);

            DuplicateBackupFinder finder = new DuplicateBackupFinder();

            var duplicates = finder.Find(backups);

            string message = "";

            foreach (var item in duplicates)
            {
                message += item.ProjectName +
                           "   " +
                           item.BackupDate.ToShortDateString() +
                           "   " +
                           item.Count +
                           " backups" +
                           System.Environment.NewLine;
            }

            MessageBox.Show(message);
        }
    }
}
