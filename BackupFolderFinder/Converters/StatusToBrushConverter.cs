using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BackupFolderFinder.Converters
{
    public class StatusToBrushConverter : IValueConverter
    {
        private static readonly Brush KeepBg = new SolidColorBrush(Color.FromRgb(230, 244, 234));       // Light green
        private static readonly Brush KeepFg = new SolidColorBrush(Color.FromRgb(19, 115, 51));        // Dark green
        private static readonly Brush KeepBorder = new SolidColorBrush(Color.FromRgb(180, 225, 195));

        private static readonly Brush ProtectedBg = new SolidColorBrush(Color.FromRgb(232, 240, 254));  // Light blue
        private static readonly Brush ProtectedFg = new SolidColorBrush(Color.FromRgb(26, 115, 232));  // Dark blue
        private static readonly Brush ProtectedBorder = new SolidColorBrush(Color.FromRgb(195, 218, 254));

        private static readonly Brush DeleteBg = new SolidColorBrush(Color.FromRgb(252, 232, 230));     // Light red
        private static readonly Brush DeleteFg = new SolidColorBrush(Color.FromRgb(197, 34, 31));       // Dark red
        private static readonly Brush DeleteBorder = new SolidColorBrush(Color.FromRgb(245, 195, 192));

        private static readonly Brush DefaultBg = new SolidColorBrush(Color.FromRgb(241, 243, 244));
        private static readonly Brush DefaultFg = new SolidColorBrush(Color.FromRgb(95, 99, 104));
        private static readonly Brush DefaultBorder = new SolidColorBrush(Color.FromRgb(218, 220, 224));

        static StatusToBrushConverter()
        {
            KeepBg.Freeze();
            KeepFg.Freeze();
            KeepBorder.Freeze();
            ProtectedBg.Freeze();
            ProtectedFg.Freeze();
            ProtectedBorder.Freeze();
            DeleteBg.Freeze();
            DeleteFg.Freeze();
            DeleteBorder.Freeze();
            DefaultBg.Freeze();
            DefaultFg.Freeze();
            DefaultBorder.Freeze();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            string part = parameter as string ?? "Background"; // "Background", "Foreground", "Border"

            switch (status)
            {
                case "Keep":
                    return part == "Foreground" ? KeepFg : (part == "Border" ? KeepBorder : KeepBg);
                case "Protected":
                    return part == "Foreground" ? ProtectedFg : (part == "Border" ? ProtectedBorder : ProtectedBg);
                case "Delete":
                    return part == "Foreground" ? DeleteFg : (part == "Border" ? DeleteBorder : DeleteBg);
                default:
                    return part == "Foreground" ? DefaultFg : (part == "Border" ? DefaultBorder : DefaultBg);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
