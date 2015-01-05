using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace DoubanFM.Desktop.Audio.Tests
{
    [TestClass]
    public class BassEngineTests
    {
        private BassEngine player;

        [TestInitialize]
        public void BassEngineSetup()
        {
            player = BassEngine.Instance;
            player.OpenFile(@"C:\You Raise Me Up.mp3");
        }

        [TestCleanup]
        public void BassEngineCleanUp()
        {
            player = null;
        }


        [TestMethod]
        public void BassEngineSingletonTest()
        {
            var player1 = BassEngine.Instance;
            var player2 = BassEngine.Instance;
            Assert.AreSame(player1, player2);
        }

        [TestMethod]
        public void BassEngineOpenLocalMp3FileTest()
        {
            player.Play();
            Assert.IsTrue(player.IsPlaying);
            if (player.IsPlaying)
                player.Stop();
        }

        [TestMethod]
        public void BassEngineOpenUrlTest()
        {
            player.Stop();
            player.OpenUrl("http://mr4.douban.com/201412202338/4fb9ab5029d055808ddc1733e62cc044/view/song/small/p1742971_1v.mp3");
            player.Play();
            Assert.IsTrue(player.IsPlaying);
            Thread.Sleep(15000);
                player.Pause();

            Thread.Sleep(3000);
            player.Play();

            Thread.Sleep(120000);
            player.Stop();
        }

        [TestMethod]
        public void BassEnginePauseTest()
        {
            if (!player.IsPlaying)
            {
                player.Play();
            }
            player.Pause();
            Assert.IsFalse(player.IsPlaying);
        }

        [TestMethod]
        public void BassEngineStopTest()
        {
            if (!player.IsPlaying)
            {
                player.Play();
            }
            player.Stop();
            Assert.IsFalse(player.IsPlaying);
        }

    }
}
