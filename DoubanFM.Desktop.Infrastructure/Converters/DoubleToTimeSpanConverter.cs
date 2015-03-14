using System;
using System.Windows.Data;

namespace DoubanFM.Desktop.Infrastructure.Converters
{
	public class DoubleToTimeSpanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;

			double position = 0.0;
			double.TryParse(value.ToString(), out position);
			return TimeSpan.FromSeconds(position);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
