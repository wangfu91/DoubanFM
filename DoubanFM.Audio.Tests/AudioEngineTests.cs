using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DoubanFM.Audio.Tests
{
    [TestClass]
    public class AudioEngineTests
    {
        private NAudioEngine player;

        [TestInitialize]
        private async Task PlayerSetup()
        {
            player = NAudioEngine.Instance;
            await player.Initialize(@"C:\Users\WangFu\Desktop\You Raise Me Up.mp3");
        }


        [TestMethod]
        public void PlayerSingletonTest()
        {
            var player1 = NAudioEngine.Instance;
            var player2 = NAudioEngine.Instance;
            Assert.AreSame(player1, player2);
        }

        [TestMethod]
        public async Task PlayLocalMp3FileTest()
        {

            await player.Play();
            await Task.Delay(5000);
            Assert.IsTrue(player.IsPlaying);

        }

        [TestMethod]
        public async Task PauseTest()
        {
            await player.Pause();
            Assert.IsFalse(player.IsPlaying);
        }
    }
}
