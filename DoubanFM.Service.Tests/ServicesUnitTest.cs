using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DoubanFM.Service.Tests
{
    [TestClass]
    public class ServicesUnitTest
    {

        [TestMethod]
        public async Task GetChannelsTest()
        {
            var channelService = new ChannelService();
            var channelList = await channelService.GetChannels();
            Assert.IsNotNull(channelList);
            Assert.IsTrue(channelList.Channels.Count> 0);
        }

        [TestMethod]
        public async Task GetSongsByChannelTest()
        {
            var channelService = new ChannelService();
            var songs = await channelService.GetSongs("1");
            Assert.IsNotNull(songs);
        }


        [TestMethod]
        public async Task LoginWithEmailTest()
        {
            var userSerice = new UserService();
            var loginInfo = await userSerice.LoginWithEmail("wangfu91@hotmail.com", "wf19912012");
            Assert.IsNotNull(loginInfo);
            if (string.IsNullOrEmpty(loginInfo.Token))
            {
                Assert.Fail(loginInfo.Err);
            }
        }

        [TestMethod]
        public async Task LoginWithUserNameTest()
        {
            var userService = new UserService();
            var loginInfo = await userService.LoginWithUserName("Coding4u", "wf19912012");
            Assert.IsNotNull(loginInfo);
            if (string.IsNullOrEmpty(loginInfo.Token))
            {
                Assert.Fail(loginInfo.Err);
            }
        }

        [TestMethod]
        public async Task GetUsesrInfoTest()
        {
            var userService = new UserService();
            var user = await userService.GetUserInfo("67242159", "7c2c65101c", "1434431471");
            Assert.IsNotNull(user);
            Assert.IsFalse(string.IsNullOrEmpty(user.Name));
        }
    }
}
