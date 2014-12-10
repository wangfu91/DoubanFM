using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DoubanFM.Common.Converters
{
    public class DoubleToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double position = 0.0;
            if (double.TryParse(value.ToString(), out position))
            {
                return TimeSpan.FromSeconds(position);
            }

            return TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
