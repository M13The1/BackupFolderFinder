using System;
using System.Collections.Generic;
using System.Text;
using BackupFolderFinder.Models;

namespace BackupFolderFinder.Services
{
    public static class BackupRules
    {
        /// <summary>
        /// Phrases that indicate a backup should never be deleted.
        /// Add new phrases here as required.
        /// </summary>
        private static readonly List<string> ProtectedPhrases =
            new List<string>
            {
                "dont delete",
                "do not delete"
            };

        /// <summary>
        /// Returns true if the supplied folder name contains a protected phrase.
        /// Matching is case-insensitive and ignores separators.
        /// </summary>
        public static bool IsProtectedFolder(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                return false;

            string normalized = Normalize(folderName);

            foreach (string phrase in ProtectedPhrases)
            {
                if (normalized.Contains(Normalize(phrase)))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a backup may be deleted.
        /// </summary>
        public static bool CanDeleteBackup(BackupInfo backup)
        {
            if (backup == null)
                return false;

            return !backup.IsProtected;
        }

        /// <summary>
        /// Converts text into a comparable form.
        /// Examples:
        ///     DontDelete      -> dont delete
        ///     Dont_Delete     -> dont delete
        ///     Do-Not-Delete   -> do not delete
        /// </summary>
        private static string Normalize(string value)
        {
            value = value.ToLowerInvariant();

            StringBuilder builder = new StringBuilder();

            foreach (char c in value)
            {
                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append(' ');
                }
            }

            string normalized = builder.ToString();

            while (normalized.Contains("  "))
                normalized = normalized.Replace("  ", " ");

            normalized = normalized.Trim();

            // Split PascalCase-like compound words after normalization isn't possible,
            // so explicitly handle the common concatenated forms.
            normalized = normalized.Replace("dontdelete", "dont delete");
            normalized = normalized.Replace("donotdelete", "do not delete");
            normalized = normalized.Replace("donot", "do not");

            return normalized;
        }
    }
}