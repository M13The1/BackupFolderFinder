using System;
using System.Collections.Generic;
using System.Linq;
using BackupFolderFinder.Common;

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

        public string BackupDateFormatted => BackupDate.ToString("dd-MMM-yyyy");

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

        public string KeepTime => KeepBackup != null ? KeepBackup.Time : string.Empty;

        public string DeleteTimes
        {
            get
            {
                if (DeleteBackups == null || DeleteBackups.Count == 0)
                    return string.Empty;

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
                if (ProtectedBackups == null || ProtectedBackups.Count == 0)
                    return string.Empty;

                List<string> times = new List<string>();
                foreach (BackupInfo backup in ProtectedBackups)
                    times.Add(backup.Time);

                return string.Join(", ", times);
            }
        }

        public int DeleteCount => DeleteBackups?.Count ?? 0;

        public int ProtectedCount => ProtectedBackups?.Count ?? 0;

        public int TotalCount => (KeepBackup != null ? 1 : 0) + DeleteCount + ProtectedCount;

        /// <summary>
        /// Total storage size in bytes of candidate backups eligible for deletion.
        /// </summary>
        public long ReclaimableSizeBytes => DeleteBackups?.Sum(b => b.SizeBytes) ?? 0;

        /// <summary>
        /// Formatted reclaimable storage size (e.g. 1.25 GB).
        /// </summary>
        public string ReclaimableSizeFormatted => ByteSizeFormatter.Format(ReclaimableSizeBytes);

        /// <summary>
        /// Total storage size in bytes of all backups on this date.
        /// </summary>
        public long TotalSizeBytes =>
            (KeepBackup?.SizeBytes ?? 0) +
            (DeleteBackups?.Sum(b => b.SizeBytes) ?? 0) +
            (ProtectedBackups?.Sum(b => b.SizeBytes) ?? 0);

        /// <summary>
        /// Formatted total storage size of all backups on this date.
        /// </summary>
        public string TotalSizeFormatted => ByteSizeFormatter.Format(TotalSizeBytes);
    }
}
