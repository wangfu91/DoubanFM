using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DoubanFM.Desktop.ResourceLibrary.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (string)parameter == "Like" ?
                value is bool && (bool)value ?
                new BitmapImage(new Uri("../Assets/Red-Heart.png", UriKind.Relative)) :
                new BitmapImage(new Uri("../Assets/Heart.png", UriKind.Relative))
                :
                value is bool && (bool)value ?
                new BitmapImage(new Uri("../Assets/Media-Pause.png", UriKind.Relative)) :
                new BitmapImage(new Uri("../Assets/Media-Play.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
