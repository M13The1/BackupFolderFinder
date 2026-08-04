using System;
using System.Text.RegularExpressions;

namespace BackupFolderFinder.Services
{
    public static class BackupRules
    {
        // Matches:
        // DontDelete
        // Dont Delete
        // Dont_Delete
        // Dont-Delete
        // DoNotDelete
        // Do Not Delete
        // Do_Not_Delete
        // Do-Not-Delete
        private static readonly Regex ProtectedFolderRegex =
            new Regex(
                @"(dont[\s_-]*delete|do[\s_-]*not[\s_-]*delete)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Returns true if the backup folder should never be deleted.
        /// </summary>
        public static bool IsProtectedFolder(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                return false;

            return ProtectedFolderRegex.IsMatch(folderName);
        }

        /// <summary>
        /// Returns true if this backup is eligible for deletion.
        /// </summary>
        public static bool CanDeleteBackup(BackupInfo backup)
        {
            if (backup == null)
                return false;

            if (string.IsNullOrWhiteSpace(backup.BackupFolder))
                return false;

            string timeFolder =
                System.IO.Path.GetFileName(
                    System.IO.Path.GetDirectoryName(backup.BackupFolder));

            if (string.IsNullOrWhiteSpace(timeFolder))
                return false;

            return !IsProtectedFolder(timeFolder);
        }
    }
}