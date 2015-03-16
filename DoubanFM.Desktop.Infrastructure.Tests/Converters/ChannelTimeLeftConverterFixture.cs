using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoubanFM.Desktop.Infrastructure.Converters;

namespace DoubanFM.Desktop.Infrastructure.Tests.Converters
{
	[TestClass]
	public class ChannelTimeLeftConverterFixture
	{
		private ChannelTimeLeftConverter converter;

		[TestInitialize]
		public void Initialize()
		{
			converter = new ChannelTimeLeftConverter();
		}

		[TestMethod]
		public void ShouldConvertTwoDoubleToTimeSpan()
		{
			object[] source = new object[] { 453.6, 234.5 };
			var result = converter.Convert(source, typeof(object[]), null, null);
			Assert.IsInstanceOfType(result, typeof(TimeSpan));
			var expected = new TimeSpan(0, 0, 3, 39, 100);
			Assert.AreEqual<TimeSpan>(expected, (TimeSpan)result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentExpectionIfSourceArrayLengthIsNotTwo()
		{
			object[] source = new object[] { 453.6 };
			converter.Convert(source, typeof(object[]), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(NotImplementedException))]
		public void ShouldThrowNotImplementedExceptionInConvertBack()
		{
			var convertedValue = converter.ConvertBack(null, null, null, null);
		}

		[TestCleanup]
		public void Cleanup()
		{
			if (converter != null)
				converter = null;
		}
	}
}
