using System.Collections.Generic;
using System.Linq;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public class DuplicateBackupFinder
    {
        public List<DuplicateBackupGroup> Find(List<BackupInfo> backups)
        {
            return backups
                .GroupBy(b => new
                {
                    b.ProjectName,
                    b.BackupDate
                })
                .Where(g => g.Count() > 1)
                .Select(g => new DuplicateBackupGroup
                {
                    ProjectName = g.Key.ProjectName,
                    BackupDate = g.Key.BackupDate,
                    Count = g.Count(),
                    Times = string.Join(", ",
                        g.OrderBy(x => x.Time)
                         .Select(x => x.Time))
                })
                .OrderBy(x => x.ProjectName)
                .ThenBy(x => x.BackupDate)
                .ToList();
        }
    }
}