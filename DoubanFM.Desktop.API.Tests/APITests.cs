using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Tests
{
    [TestClass]
    public class ServicesUnitTest
    {
        private LoginResult loginResult;

        [TestInitialize]
        public void Initialize()
        {
            loginResult = new LoginResult
            {
                UserId = "67242159",
                Token = "7c2c65101c",
                Expire = "1434431471"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public async Task GetChannelsTest()
        {
            var channelService = new ChannelService();
            var channelList = await channelService.GetChannels();
            Assert.IsNotNull(channelList);
            Assert.IsTrue(channelList.Channels.Count > 0);
        }

        [TestMethod]
        public async Task GetChannelsAfterLoginTest()
        {
            var channelService = new ChannelService(loginResult);
            var channelList = await channelService.GetChannels();
            Assert.IsNotNull(channelList);
            Assert.IsTrue(channelList.Channels.Count > 0);

        }

        [TestMethod]
        public async Task GetSongsByChannelTest()
        {
            var songService = new SongService();
            var songs = await songService.GetSongs(1);
            Assert.IsNotNull(songs);
        }

        [TestMethod]
        public async Task GetLikedSongsTest()
        {
            var songService = new SongService(loginResult);
            var songs = await songService.GetSongs(-3);
            Assert.IsNotNull(songs);
        }


        [TestMethod]
        public async Task LoginWithEmailTest()
        {
            var loginSerice = new LoginService();
            var loginInfo = await loginSerice.LoginWithEmail("wangfu91@hotmail.com", "wf19912012");
            Assert.IsNotNull(loginInfo);
            if (string.IsNullOrEmpty(loginInfo.Token))
            {
                Assert.Fail(loginInfo.Err);
            }
        }

        [TestMethod]
        public async Task LoginWithUserNameTest()
        {
            var loginSerice = new LoginService();
            var loginInfo = await loginSerice.LoginWithUserName("Coding4u", "wf19912012");
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

        [TestMethod]
        public async Task LikeASongTest()
        {
            var songService = new SongService(loginResult);
            var result = await songService.Like("1742969", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task UnlikeASongTest()
        {
            var songService = new SongService(loginResult);

            var result = await songService.Unlike("1742969", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task BanASongTest()
        {
            var songService = new SongService(loginResult);
            var result = await songService.Ban("1671513", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task NormalEndASongTest()
        {
            var songService = new SongService(loginResult);
            var result = await songService.NormalEnd("1742969", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task SkipASongTest()
        {
            var songService = new SongService(loginResult);
            var result = await songService.Skip("1742969", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task GetDoubanLyricsTest()
        {
            var lyricsService = new LyricsService();
            var song = new Song { SID = "1742965" };
            var lyrics = await lyricsService.GetLyrics(song.SID);
            Assert.IsNotNull(lyrics);
        }

    }

}
