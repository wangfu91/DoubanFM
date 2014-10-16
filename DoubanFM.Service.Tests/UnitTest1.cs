using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubanFM.Service.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public async void ChannelTest()
        {
            var channelService = new ChannelService();
            var content = await channelService.GetChannels(1, "n");
            var len = content.Count;
        }
    }
}
