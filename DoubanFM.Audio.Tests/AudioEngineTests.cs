using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DoubanFM.Audio.Tests
{
    [TestClass]
    public class AudioEngineTests
    {
        private NAudioEngine player;

        [TestInitialize]
        private void PlayerSetup()
        {
            player = NAudioEngine.Instance;
            player.OpenFile(@"C:\Users\WangFu\Desktop\You Raise Me Up.mp3");
        }


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

            player.Play();
            Assert.IsTrue(player.IsPlaying);

        }

        [TestMethod]
        public void PauseTest()
        {
            player.Pause();
            Assert.IsFalse(player.IsPlaying);
        }
    }
}
