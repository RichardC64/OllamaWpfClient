using System.Windows.Data;

namespace OllamaWpfClient;

public class InvertedBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter,
        System.Globalization.CultureInfo culture)
    {
        if (value is not bool bValue) return false;
   
        return !bValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter,
        System.Globalization.CultureInfo culture)
    {
        return null;
    }

}