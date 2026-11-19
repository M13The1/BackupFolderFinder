using System;
using System.IO;

namespace BackupFolderFinder.Common
{
    public static class ByteSizeFormatter
    {
        public static string Format(long bytes)
        {
            if (bytes < 0)
                return "0 B";

            if (bytes < 1024)
                return $"{bytes} B";

            double kb = bytes / 1024.0;
            if (kb < 1024)
                return $"{kb:F1} KB";

            double mb = kb / 1024.0;
            if (mb < 1024)
                return $"{mb:F1} MB";

            double gb = mb / 1024.0;
            return $"{gb:F2} GB";
        }

        public static long CalculateDirectorySize(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
                return 0;

            long totalBytes = 0;
            try
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                foreach (var file in directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    try
                    {
                        totalBytes += file.Length;
                    }
                    catch
                    {
                        // Ignore locked or inaccessible single files
                    }
                }
            }
            catch
            {
                // Inaccessible directory
            }

            return totalBytes;
        }
    }
}
