using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using DoubanFM.Universal.APIs.Services;

namespace DoubanFM.Universal.API.Tests
{
    [TestClass]
    public class APITests
    {
        [TestMethod]
        public async Task ChannelServiceTest()
        {
            //Arrange 
            var service = new ChannelService();

            //Act
            var result = await service.GetChannels();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Channels.Count > 0);

        }
    }
}
