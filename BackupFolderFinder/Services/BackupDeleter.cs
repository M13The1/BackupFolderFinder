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

            if (group == null)
                return 0;

            int deletedCount = 0;

            foreach (BackupInfo backup in group.DeleteBackups)
            {
                try
                {
                    // Extra safety.
                    if (backup.IsProtected)
                    {
                        errors.Add(
                            "Protected backup was skipped:" +
                            Environment.NewLine +
                            backup.TimeFolder);

                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(backup.TimeFolder))
                    {
                        errors.Add(
                            "Time folder not found for backup:" +
                            Environment.NewLine +
                            backup.BackupFolder);

                        continue;
                    }

                    if (!Directory.Exists(backup.TimeFolder))
                    {
                        continue;
                    }

                    Directory.Delete(backup.TimeFolder, true);

                    deletedCount++;
                }
                catch (Exception ex)
                {
                    errors.Add(
                        backup.TimeFolder +
                        Environment.NewLine +
                        ex.Message);
                }
            }

            return deletedCount;
        }
    }
}