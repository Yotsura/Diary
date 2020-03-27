using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dialy.ValueConverter
{
    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            return date == DateTime.Parse("0001/01/01 0:00:00") ? string.Empty : date.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value.ToString();
            if (DateTime.TryParse(strValue, out var resultDateTime))
            {
                return resultDateTime;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
