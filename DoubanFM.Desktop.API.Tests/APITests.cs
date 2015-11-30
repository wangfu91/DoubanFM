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
                DoubanUserId = "67242159",
                AccessToken = "a28eb0e67d934cf0e7ac12b378b9e6d0",
                ExpireIn = 7776000
            };
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public async Task GetAppChannelsTest()
        {
            var channelService = new ChannelService();
            channelService.AccessToken = loginResult.AccessToken;
            var channelGroupList = await channelService.GetAppChannels();
            Assert.IsNotNull(channelGroupList);
            Assert.IsTrue(channelGroupList.Groups.Count > 0);
        }


        [TestMethod]
        public async Task GetPlayListByChannelTest()
        {
            var songService = new SongService();
            songService.AccessToken = loginResult.AccessToken;
            var songs = await songService.GetPlayList(1);
            Assert.IsNotNull(songs.Songs);
        }

        [TestMethod]
        public async Task GetLikedSongsTest()
        {
            var songService = new SongService();
            songService.AccessToken = loginResult.AccessToken;
            var songs = await songService.GetPlayList(-3);
            Assert.IsNotNull(songs);
        }


        [TestMethod]
        public async Task LoginWithEmailTest()
        {
            var loginSerice = new LoginService();
            var loginInfo = await loginSerice.LoginWithEmail("wangfu91@hotmail.com", "wf19912012");
            Assert.IsNotNull(loginInfo);
            if (string.IsNullOrEmpty(loginInfo.AccessToken))
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public async Task GetUsesrInfoTest()
        {
            var userService = new UserService();
            var user = await userService.GetUserInfo(loginResult.DoubanUserId, loginResult.AccessToken);
            Assert.IsNotNull(user);
            Assert.IsFalse(string.IsNullOrEmpty(user.Name));
        }

        [TestMethod]
        public async Task LikeASongTest()
        {
            var songService = new SongService();
            songService.AccessToken = loginResult.AccessToken;
            var result = await songService.Like("1742969", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task UnlikeASongTest()
        {
            var songService = new SongService();
            songService.AccessToken = loginResult.AccessToken;

            var result = await songService.Unlike("1742969", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task BanASongTest()
        {
            var songService = new SongService();
            songService.AccessToken = loginResult.AccessToken;

            var result = await songService.Ban("1671513", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task NormalEndASongTest()
        {
            var songService = new SongService();
            songService.AccessToken = loginResult.AccessToken;

            var result = await songService.NormalEnd("1742969", 1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.R == 0);
        }

        [TestMethod]
        public async Task SkipASongTest()
        {
            var songService = new SongService();
            songService.AccessToken = loginResult.AccessToken;

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
