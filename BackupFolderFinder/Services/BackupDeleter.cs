using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public class DeletionResult
    {
        public int DeletedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    public class BackupDeleter
    {
        public int Delete(BackupRetentionGroup group, out List<string> errors)
        {
            var result = DeleteGroup(group);
            errors = result.Errors;
            return result.DeletedCount;
        }

        public DeletionResult DeleteGroup(BackupRetentionGroup group)
        {
            var result = new DeletionResult();
            if (group == null || group.DeleteBackups == null)
                return result;

            foreach (BackupInfo backup in group.DeleteBackups)
            {
                try
                {
                    // Extra safety
                    if (backup.IsProtected)
                    {
                        result.SkippedCount++;
                        result.Errors.Add($"Protected backup was skipped:{Environment.NewLine}{backup.TimeFolder}");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(backup.TimeFolder))
                    {
                        result.Errors.Add($"Time folder path is empty for backup:{Environment.NewLine}{backup.BackupFolder}");
                        continue;
                    }

                    if (!Directory.Exists(backup.TimeFolder))
                    {
                        continue;
                    }

                    Directory.Delete(backup.TimeFolder, true);
                    result.DeletedCount++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"{backup.TimeFolder}:{Environment.NewLine}{ex.Message}");
                }
            }

            return result;
        }

        public DeletionResult DeleteGroups(IEnumerable<BackupRetentionGroup> groups, IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            var aggregated = new DeletionResult();
            if (groups == null)
                return aggregated;

            foreach (var group in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();
                progress?.Report($"Deleting redundant backups for {group.ProjectName} ({group.BackupDateFormatted})...");

                var single = DeleteGroup(group);
                aggregated.DeletedCount += single.DeletedCount;
                aggregated.SkippedCount += single.SkippedCount;
                aggregated.Errors.AddRange(single.Errors);
            }

            return aggregated;
        }

        public Task<DeletionResult> DeleteGroupAsync(BackupRetentionGroup group, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => DeleteGroup(group), cancellationToken);
        }

        public Task<DeletionResult> DeleteGroupsAsync(IEnumerable<BackupRetentionGroup> groups, IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => DeleteGroups(groups, progress, cancellationToken), cancellationToken);
        }
    }
}
