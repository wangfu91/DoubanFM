using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DoubanFM.Universal.API.Services;

namespace DoubanFM.Universal.API.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetChannelsTest()
        {
            var channelService = new ChannelService();
            var channelList = await channelService.GetChannels();
            Assert.IsNotNull(channelList);
            Assert.IsTrue(channelList.Channels.Count > 0);
        }
    }
}
