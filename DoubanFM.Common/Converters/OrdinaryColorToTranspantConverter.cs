using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DoubanFM.Common.Converters
{
    /// <summary>
    /// Convert ordinary color to transpant color with same RGB value.
    /// </summary>
    public class OrdinaryColorToTranspantConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Color.FromArgb(0, ((Color)value).R, ((Color)value).G, ((Color)value).B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
