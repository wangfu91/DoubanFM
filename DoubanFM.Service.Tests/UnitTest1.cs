using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DoubanFM.Service.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cs1 = new Class1();

            //var chanels = cs1.Get("j/app/radio/channels");

            var people = cs1.Post("/j/app/radio/people");
        }
    }
}
