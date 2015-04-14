using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DoubanFM.Desktop.Infrastructure.Converters
{
    public class UriToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is string)
            {
                value = new Uri((string)value);
            }

            if (value is Uri)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.DecodePixelWidth = 80;
                //bi.DecodePixelHeight = 60;                
                bi.UriSource = (Uri)value;
                bi.EndInit();
                return bi;
            }

            return null;


            //Uri uri;
            //if (value == null || string.IsNullOrEmpty(value.ToString()))
            //{
            //    uri = new Uri("pack://application:,,,/DoubanFM.Desktop.Resource;component/Assets/DoubanFM_NoCover.png");
            //}
            //else
            //{
            //    uri = new Uri(value.ToString());
            //}

            //var bmp = new BitmapImage();
            //bmp.BeginInit();
            //bmp.DecodePixelWidth = 300;
            //bmp.DecodePixelHeight = 300;
            //bmp.UriSource = uri;
            //bmp.EndInit();

            //return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
