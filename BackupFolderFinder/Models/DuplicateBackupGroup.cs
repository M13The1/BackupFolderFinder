using System;

namespace BackupFolderFinder.Models
{
    public class DuplicateBackupGroup
    {
        public string ProjectName { get; set; }

        public DateTime BackupDate { get; set; }

        public int Count { get; set; }

        public string Times { get; set; }
    }
}