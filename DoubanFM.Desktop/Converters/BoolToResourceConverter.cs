using System;
using System.Windows.Data;

namespace DoubanFM.Desktop.Converters
{
    public class BoolToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ("Like" == (string)parameter)
            {
                if ((bool)value)
                {
                    return App.Current.Resources["LikedIcon"];
                }

                return App.Current.Resources["NotLikedIcon"];
            }


            if ((bool)value)
            {
                return App.Current.Resources["PauseIcon"];
            }

            return App.Current.Resources["PlayIcon"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
