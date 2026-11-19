using System;
using System.Reflection;

namespace BackupFolderFinder.Common
{
    public static class AppInfo
    {
        public static string AppName => "Backup Folder Finder";

        public static string Version
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                return ver != null ? $"{ver.Major}.{ver.Minor}.{ver.Build}" : "1.0.0";
            }
        }

        public static string BuildNumber
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                if (ver != null)
                {
                    return ver.Revision > 0 
                        ? $"Build {ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}"
                        : $"Build {ver.Major}.{ver.Minor}.{ver.Build}";
                }
                return "Build 1.0.2611.19";
            }
        }

        public static string AuthorTag => "M13";

        public static string WindowTitle => $"{AppName} — {BuildNumber}";
    }
}
