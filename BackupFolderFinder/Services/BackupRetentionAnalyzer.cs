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

                retention.KeepBackup = ordered.First();

                retention.DeleteBackups =
                    ordered.Skip(1).ToList();

                result.Add(retention);
            }

            return result;
        }
    }
}