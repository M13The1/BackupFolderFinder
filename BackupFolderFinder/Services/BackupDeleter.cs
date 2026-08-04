using System.IO;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public class BackupDeleter
    {
        public void Delete(BackupRetentionGroup group)
        {
            foreach (BackupInfo backup in group.DeleteBackups)
            {
                if (Directory.Exists(backup.BackupFolder))
                {
                    Directory.Delete(backup.BackupFolder, true);
                }
            }
        }
    }
}