using System;
using System.Windows.Data;

namespace DoubanFM.Desktop.Infrastructure.Converters
{
	public class ChannelTimeLeftConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (values == null)
				return null;

			double channelLength = 0.0;
			double channelPosition = 0.0;
			if (values.Length == 2)
			{
				double.TryParse(values[0].ToString(), out channelLength);
				double.TryParse(values[1].ToString(), out channelPosition);
			}
			else
			{
				throw new ArgumentException(string.Format("Length of the values array is incorrect, expect={0}, actual={1}", 2, values.Length), "values");
			}
			var timeLeft = channelLength - channelPosition;
			return TimeSpan.FromSeconds(timeLeft);
			//return string.Format("-{0:m\\:ss}", ts);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
