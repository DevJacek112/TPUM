using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ViewModel;

public class BoolToVisibilityConverter : IValueConverter
{
    public bool Inverse { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool flag = (bool)value;
        if (Inverse)
            flag = !flag;
        return flag ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((Visibility)value) == Visibility.Visible;
    }
}