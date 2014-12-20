using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DoubanFM.Audio.Tests
{
    [TestClass]
    public class AudioEngineTests
    {
        //private BassEngine player;

        //[TestInitialize]
        //private void PlayerSetup()
        //{
        //    player = BassEngine.Instance;
        //    player.OpenFile(@"C:\You Raise Me Up.mp3");
        //}


        [TestMethod]
        public void PlayerSingletonTest()
        {
            var player1 = NAudioEngine.Instance;
            var player2 = NAudioEngine.Instance;
            Assert.AreSame(player1, player2);
        }

        [TestMethod]
        public void PlayLocalMp3FileTest()
        {
            var player = NAudioEngine.Instance;
            player.OpenFile(@"C:\You Raise Me Up.mp3");
            player.Play();
            Assert.IsTrue(player.IsPlaying);
        }

        [TestMethod]
        public void PauseTest()
        {
            var player = NAudioEngine.Instance;
            player.OpenFile(@"C:\You Raise Me Up.mp3");
            if (!player.IsPlaying)
            {
                player.Play();
            }
            player.Pause();
            Assert.IsFalse(player.IsPlaying);
        }
    }
}
