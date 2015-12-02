using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DoubanFM.Desktop.Infrastructure.Converters
{
    public class StringToColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorStr = value.ToString();
            return new SolidColorBrush(Parse(colorStr));
;        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static Color Parse(string color)
        {
            var r = Byte.Parse(color.Substring(2 , 2), NumberStyles.HexNumber);
            var g = Byte.Parse(color.Substring(4 , 2), NumberStyles.HexNumber);
            var b = Byte.Parse(color.Substring(6 , 2), NumberStyles.HexNumber);

            //return Color.FromArgb(a, r, g, b);
            return Color.FromRgb(r, g, b);
        }
    }
}
