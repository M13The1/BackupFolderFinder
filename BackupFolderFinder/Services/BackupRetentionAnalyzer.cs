using System.Collections.Generic;
using System.Linq;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public class BackupRetentionAnalyzer
    {
        public List<BackupRetentionGroup> Analyze(List<BackupInfo> backups)
        {
            List<BackupRetentionGroup> result =
                new List<BackupRetentionGroup>();

            var groups = backups
                .GroupBy(b => new
                {
                    b.ProjectName,
                    b.BackupDate
                });

            foreach (var group in groups)
            {
                List<BackupInfo> ordered =
                    group.OrderBy(b => b.Time).ToList();

                if (ordered.Count <= 1)
                    continue;

                BackupRetentionGroup retention =
                    new BackupRetentionGroup();

                retention.ProjectName = group.Key.ProjectName;
                retention.BackupDate = group.Key.BackupDate;

                // -----------------------------------------------------------------
                // Keep the earliest backup
                // -----------------------------------------------------------------

                retention.KeepBackup = ordered.First();
                retention.KeepBackup.Status = "Keep";

                retention.DisplayBackups.Add(retention.KeepBackup);

                // -----------------------------------------------------------------
                // Process remaining backups
                // -----------------------------------------------------------------

                for (int i = 1; i < ordered.Count; i++)
                {
                    BackupInfo backup = ordered[i];

                    if (backup.IsProtected)
                    {
                        backup.Status = "Protected";

                        retention.ProtectedBackups.Add(backup);
                        retention.DisplayBackups.Add(backup);
                    }
                    else
                    {
                        backup.Status = "Delete";

                        retention.DeleteBackups.Add(backup);
                        retention.DisplayBackups.Add(backup);
                    }
                }

                // Always display backups in time order.
                retention.DisplayBackups =
                    retention.DisplayBackups
                        .OrderBy(b => b.Time)
                        .ToList();

                // Only display groups requiring user action.
                if (retention.DeleteBackups.Count > 0)
                {
                    result.Add(retention);
                }
            }

            return result;
        }
    }
}