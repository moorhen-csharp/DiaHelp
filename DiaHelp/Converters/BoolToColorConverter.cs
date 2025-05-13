using System.Globalization;

namespace DiaHelp.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive && parameter is string colors)
            {
                var colorArray = colors.Split(',');
                if (colorArray.Length == 2)
                {
                    return isActive ? Color.FromArgb(colorArray[1]) : Color.FromArgb(colorArray[0]);
                }
            }
            return Color.FromArgb("#2d2d2f");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 