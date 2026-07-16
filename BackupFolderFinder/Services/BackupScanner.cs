using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public class BackupScanner
    {
        public List<BackupInfo> Scan(string rootFolder)
        {
            List<BackupInfo> backups = new List<BackupInfo>();

            // Project folders
            foreach (string projectFolder in Directory.GetDirectories(rootFolder))
            {
                string projectName = Path.GetFileName(projectFolder);

                // YYYYMM folders
                foreach (string monthFolder in Directory.GetDirectories(projectFolder))
                {
                    string monthName = Path.GetFileName(monthFolder);

                    // DD folders
                    foreach (string dayFolder in Directory.GetDirectories(monthFolder))
                    {
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

                        // HHMM folders
                        foreach (string timeFolder in Directory.GetDirectories(dayFolder))
                        {
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

                            backups.Add(new BackupInfo
                            {
                                ProjectName = projectName,
                                BackupDate = backupDate,
                                Time = time,
                                BackupFolder = backupFolder,
                                Db1File = db1File
                            });
                        }
                    }
                }
            }

            return backups;
        }
    }
}