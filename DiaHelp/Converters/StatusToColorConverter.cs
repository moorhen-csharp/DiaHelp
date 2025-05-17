using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace DiaHelp.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string selected && parameter is string current)
            {
                // Цвет при совпадении
                var selectedColor = Color.FromArgb("#50c878"); // Золотой
                var defaultColor = Colors.Gray;

                return selected == current ? selectedColor : defaultColor;
            }

            return Colors.Gray; // Цвет по умолчанию
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}