using System;

namespace BackupFolderFinder.Models
{
    public class BackupInfo
    {
        /// <summary>
        /// Project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Backup date.
        /// </summary>
        public DateTime BackupDate { get; set; }

        /// <summary>
        /// HHMM folder name.
        /// Example: 1015
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Full path to HHMM folder.
        /// </summary>
        public string TimeFolder { get; set; }

        /// <summary>
        /// Full path to Project_bak folder.
        /// </summary>
        public string BackupFolder { get; set; }

        /// <summary>
        /// Full path to DB1 file.
        /// </summary>
        public string Db1File { get; set; }

        /// <summary>
        /// True if backup is protected from deletion.
        /// </summary>
        public bool IsProtected { get; set; }

        /// <summary>
        /// Display status.
        /// Keep / Protected / Delete
        /// </summary>
        public string Status { get; set; }

        public string FolderName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TimeFolder))
                    return string.Empty;

                return System.IO.Path.GetFileName(TimeFolder);
            }
        }
    }
}