using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DoubanFM.Common.Converters
{
    public class BoolToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (string)parameter == "Like"
                ?
                value is bool && (bool)value ? "/Asstes/RedHeart.png" : "/Assets/Heart.png"
                :
                value is bool && (bool)value ? "/Assets/Media-Pause.png" : "/Assets/Media-Play.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
