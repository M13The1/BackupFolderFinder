using System;
using System.Collections.Generic;

namespace BackupFolderFinder.Models
{
    public class BackupRetentionGroup
    {
        public string ProjectName { get; set; }

        public DateTime BackupDate { get; set; }

        // Backup to retain (earliest HHMM)
        public BackupInfo KeepBackup { get; set; }

        // Backups that can be deleted
        public List<BackupInfo> DeleteBackups { get; set; }

        // For DataGridView
        public string KeepTime
        {
            get { return KeepBackup != null ? KeepBackup.Time : ""; }
        }

        public string DeleteTimes
        {
            get
            {
                if (DeleteBackups == null || DeleteBackups.Count == 0)
                    return "";

                List<string> times = new List<string>();

                foreach (BackupInfo backup in DeleteBackups)
                    times.Add(backup.Time);

                return string.Join(", ", times);
            }
        }

        public int DeleteCount
        {
            get
            {
                return DeleteBackups == null ? 0 : DeleteBackups.Count;
            }
        }
    }
}