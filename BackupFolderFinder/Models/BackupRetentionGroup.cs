using System;
using System.Collections.Generic;

namespace BackupFolderFinder.Models
{
    public class BackupRetentionGroup
    {
        public BackupRetentionGroup()
        {
            DeleteBackups = new List<BackupInfo>();
            ProtectedBackups = new List<BackupInfo>();
            DisplayBackups = new List<BackupInfo>();
        }

        public string ProjectName { get; set; }

        public DateTime BackupDate { get; set; }

        /// <summary>
        /// Earliest backup.
        /// </summary>
        public BackupInfo KeepBackup { get; set; }

        /// <summary>
        /// Backups eligible for deletion.
        /// </summary>
        public List<BackupInfo> DeleteBackups { get; set; }

        /// <summary>
        /// Backups protected from deletion.
        /// </summary>
        public List<BackupInfo> ProtectedBackups { get; set; }

        /// <summary>
        /// Complete list displayed in UI.
        /// </summary>
        public List<BackupInfo> DisplayBackups { get; set; }

        public string KeepTime
        {
            get
            {
                return KeepBackup != null ? KeepBackup.Time : "";
            }
        }

        public string DeleteTimes
        {
            get
            {
                if (DeleteBackups.Count == 0)
                    return "";

                List<string> times = new List<string>();

                foreach (BackupInfo backup in DeleteBackups)
                    times.Add(backup.Time);

                return string.Join(", ", times);
            }
        }

        public string ProtectedTimes
        {
            get
            {
                if (ProtectedBackups.Count == 0)
                    return "";

                List<string> times = new List<string>();

                foreach (BackupInfo backup in ProtectedBackups)
                    times.Add(backup.Time);

                return string.Join(", ", times);
            }
        }

        public int DeleteCount
        {
            get
            {
                return DeleteBackups.Count;
            }
        }

        public int ProtectedCount
        {
            get
            {
                return ProtectedBackups.Count;
            }
        }
    }
}