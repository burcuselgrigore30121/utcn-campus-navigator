using System.Globalization;

namespace CampusNavigator.Converters
{
    /// <summary>Returneaza true daca valoarea NU e null/empty (pentru IsVisible)</summary>
    public class NotNullConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is string s ? !string.IsNullOrEmpty(s) : value != null;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>Mapeaza tipul activitatii la o culoare pentru bara laterala</summary>
    public class TipColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                "Curs"      => Color.FromArgb("#3B6FD4"), // albastru
                "Laborator" => Color.FromArgb("#E05A2B"), // portocaliu
                "Seminar"   => Color.FromArgb("#2BAE60"), // verde
                _           => Color.FromArgb("#BBBBBB"), // gri (Sport, etc)
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
