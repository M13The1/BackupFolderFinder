using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using BackupFolderFinder.Common;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public enum DeletionMode
    {
        RecycleBin,
        Permanent
    }

    public class DeletionResult
    {
        public int DeletedCount { get; set; }
        public int SkippedCount { get; set; }
        public long ReclaimedBytes { get; set; }
        public string ReclaimedSizeFormatted => ByteSizeFormatter.Format(ReclaimedBytes);
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
        public DeletionMode ModeUsed { get; set; } = DeletionMode.RecycleBin;
    }

    public class BackupDeleter
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public int wFunc;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pTo;
            public ushort fFlags;
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszProgressTitle;
        }

        private const int FO_DELETE = 0x0003;
        private const ushort FOF_ALLOWUNDO = 0x0040;
        private const ushort FOF_NOCONFIRMATION = 0x0010;
        private const ushort FOF_SILENT = 0x0004;
        private const ushort FOF_NOERRORUI = 0x0400;

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT fileOp);

        public int Delete(BackupRetentionGroup group, out List<string> errors)
        {
            var result = DeleteGroup(group, DeletionMode.RecycleBin);
            errors = result.Errors;
            return result.DeletedCount;
        }

        public DeletionResult DeleteGroup(BackupRetentionGroup group, DeletionMode mode = DeletionMode.RecycleBin)
        {
            var result = new DeletionResult { ModeUsed = mode };
            if (group == null || group.DeleteBackups == null)
                return result;

            foreach (BackupInfo backup in group.DeleteBackups)
            {
                try
                {
                    // Multi-layer safety check: Never delete a protected backup
                    if (backup.IsProtected)
                    {
                        result.SkippedCount++;
                        result.Errors.Add($"Protected backup was safeguarded and skipped:{Environment.NewLine}{backup.TimeFolder}");
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

                    long folderSize = backup.SizeBytes > 0 ? backup.SizeBytes : ByteSizeFormatter.CalculateDirectorySize(backup.TimeFolder);

                    if (mode == DeletionMode.RecycleBin)
                    {
                        DeleteToRecycleBin(backup.TimeFolder);
                    }
                    else
                    {
                        Directory.Delete(backup.TimeFolder, true);
                    }

                    result.DeletedCount++;
                    result.ReclaimedBytes += folderSize;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"{backup.TimeFolder}:{Environment.NewLine}{ex.Message}");
                }
            }

            return result;
        }

        public DeletionResult DeleteGroups(IEnumerable<BackupRetentionGroup> groups, DeletionMode mode = DeletionMode.RecycleBin, IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            var aggregated = new DeletionResult { ModeUsed = mode };
            if (groups == null)
                return aggregated;

            foreach (var group in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string modeText = mode == DeletionMode.RecycleBin ? "Recycling" : "Deleting";
                progress?.Report($"{modeText} redundant backups for {group.ProjectName} ({group.BackupDateFormatted})...");

                var single = DeleteGroup(group, mode);
                aggregated.DeletedCount += single.DeletedCount;
                aggregated.SkippedCount += single.SkippedCount;
                aggregated.ReclaimedBytes += single.ReclaimedBytes;
                aggregated.Errors.AddRange(single.Errors);
            }

            return aggregated;
        }

        public Task<DeletionResult> DeleteGroupAsync(BackupRetentionGroup group, DeletionMode mode = DeletionMode.RecycleBin, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => DeleteGroup(group, mode), cancellationToken);
        }

        public Task<DeletionResult> DeleteGroupsAsync(IEnumerable<BackupRetentionGroup> groups, DeletionMode mode = DeletionMode.RecycleBin, IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => DeleteGroups(groups, mode, progress, cancellationToken), cancellationToken);
        }

        private static void DeleteToRecycleBin(string path)
        {
            // SHFileOperation expects double null-terminated string
            var fileOp = new SHFILEOPSTRUCT
            {
                wFunc = FO_DELETE,
                pFrom = path + "\0\0",
                fFlags = FOF_ALLOWUNDO | FOF_NOCONFIRMATION | FOF_SILENT | FOF_NOERRORUI
            };

            int ret = SHFileOperation(ref fileOp);
            if (ret != 0)
            {
                throw new Win32Exception(ret, $"Failed to send directory to Recycle Bin (Error code {ret}).");
            }
        }
    }
}
