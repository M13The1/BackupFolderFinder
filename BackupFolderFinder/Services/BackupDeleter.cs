using System;
using System.Collections.Generic;
using System.IO;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public class BackupDeleter
    {
        public int Delete(BackupRetentionGroup group, out List<string> errors)
        {
            errors = new List<string>();
            int deletedCount = 0;

            foreach (BackupInfo backup in group.DeleteBackups)
            {
                try
                {
                    // BackupFolder:
                    // D:\Backup\Model1\202607\02\1015\Model1_bak

                    DirectoryInfo backupFolder =
                        new DirectoryInfo(backup.BackupFolder);

                    // HHMM folder:
                    // D:\Backup\Model1\202607\02\1015

                    DirectoryInfo timeFolder = backupFolder.Parent;

                    if (timeFolder == null)
                    {
                        errors.Add("Unable to determine time folder for: " +
                                   backup.BackupFolder);
                        continue;
                    }

                    if (!timeFolder.Exists)
                    {
                        continue;
                    }

                    timeFolder.Delete(true);

                    deletedCount++;
                }
                catch (Exception ex)
                {
                    errors.Add(
                        backup.BackupFolder +
                        Environment.NewLine +
                        ex.Message);
                }
            }

            return deletedCount;
        }
    }
}