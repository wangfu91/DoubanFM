using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoubanFM.Desktop.Infrastructure.Converters;

namespace DoubanFM.Desktop.Infrastructure.Tests.Converters
{
	/// <summary>
	/// Summary description for DoubleToTimeSpanConverterFixture
	/// </summary>
	[TestClass]
	public class DoubleToTimeSpanConverterFixture
	{
		public DoubleToTimeSpanConverterFixture()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		private DoubleToTimeSpanConverter converter = null;

		[TestInitialize]
		public void Initialize()
		{
			converter = new DoubleToTimeSpanConverter();
		}

		[TestMethod]
		public void ShouldConvertValidTimespanFromDouble()
		{
			double source = 123.45;
			var result = converter.Convert(source, typeof(double?), null, null);

			Assert.IsInstanceOfType(result, typeof(TimeSpan));
			var expect = TimeSpan.FromSeconds(source);
			Assert.AreEqual<TimeSpan>(expect, (TimeSpan)result);
		}

		[TestMethod]
		public void ShouldReturnTimeSpanZeroForInValidDouble()
		{
			var invalidSource = "test";
			var result = converter.Convert(invalidSource, typeof(double?), null, null);

			Assert.IsInstanceOfType(result, typeof(TimeSpan));
			var expect = TimeSpan.Zero;
			Assert.AreEqual<TimeSpan>(expect, (TimeSpan)result);
		}

		[TestCleanup]
		public void Cleanup()
		{
			if (converter != null)
				converter = null;
		}
	}
}
