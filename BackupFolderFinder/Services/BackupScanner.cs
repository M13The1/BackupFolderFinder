using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public class BackupScanner
    {
        public List<BackupInfo> Scan(string rootFolder, IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            List<BackupInfo> backups = new List<BackupInfo>();

            if (string.IsNullOrWhiteSpace(rootFolder) || !Directory.Exists(rootFolder))
                return backups;

            string[] projectFolders;
            try
            {
                projectFolders = Directory.GetDirectories(rootFolder);
            }
            catch (Exception ex)
            {
                progress?.Report($"Error accessing root directory: {ex.Message}");
                return backups;
            }

            // Project folders
            foreach (string projectFolder in projectFolders)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string projectName = Path.GetFileName(projectFolder);
                progress?.Report($"Scanning project: {projectName}...");

                string[] monthFolders;
                try
                {
                    monthFolders = Directory.GetDirectories(projectFolder);
                }
                catch
                {
                    continue;
                }

                // YYYYMM folders
                foreach (string monthFolder in monthFolders)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string monthName = Path.GetFileName(monthFolder);

                    string[] dayFolders;
                    try
                    {
                        dayFolders = Directory.GetDirectories(monthFolder);
                    }
                    catch
                    {
                        continue;
                    }

                    // DD folders
                    foreach (string dayFolder in dayFolders)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        string dayName = Path.GetFileName(dayFolder);

                        DateTime backupDate;
                        bool validDate = DateTime.TryParseExact(
                            monthName + dayName,
                            "yyyyMMdd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out backupDate);

                        if (!validDate)
                            continue;

                        string[] timeFolders;
                        try
                        {
                            timeFolders = Directory.GetDirectories(dayFolder);
                        }
                        catch
                        {
                            continue;
                        }

                        // HHMM folders
                        foreach (string timeFolder in timeFolders)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            string time = Path.GetFileName(timeFolder);

                            string backupFolder = Path.Combine(
                                timeFolder,
                                projectName + "_bak");

                            if (!Directory.Exists(backupFolder))
                                continue;

                            string db1File = Path.Combine(
                                backupFolder,
                                projectName + "_bak.db1");

                            if (!File.Exists(db1File))
                                continue;

                            BackupInfo backup = new BackupInfo
                            {
                                ProjectName = projectName,
                                BackupDate = backupDate,
                                Time = time,
                                TimeFolder = timeFolder,
                                BackupFolder = backupFolder,
                                Db1File = db1File,
                                IsProtected = BackupRules.IsProtectedFolder(time),
                                SizeBytes = BackupFolderFinder.Common.ByteSizeFormatter.CalculateDirectorySize(timeFolder)
                            };

                            backups.Add(backup);
                        }
                    }
                }
            }

            return backups;
        }

        public Task<List<BackupInfo>> ScanAsync(string rootFolder, IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => Scan(rootFolder, progress, cancellationToken), cancellationToken);
        }
    }
}
