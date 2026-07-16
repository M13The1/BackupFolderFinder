using System;

namespace BackupFolderFinder.Models
{
    public class BackupInfo
    {
        public string ProjectName { get; set; }

        public DateTime BackupDate { get; set; }

        public string Time { get; set; }

        public string BackupFolder { get; set; }

        public string Db1File { get; set; }
    }
}