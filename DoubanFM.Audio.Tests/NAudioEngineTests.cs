using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DoubanFM.Audio.Tests
{
    [TestClass]
    public class NAudioEngineTests
    {
        private NAudioEngine player;

        [TestInitialize]
        public void NAudioEngineSetup()
        {
            player = NAudioEngine.Instance;
            player.OpenFile(@"C:\You Raise Me Up.mp3");
        }

        [TestCleanup]
        public void NAudioEngineCleanUp()
        {
            player = null;
        }

        [TestMethod]
        public void NAudioEngineSingletonTest()
        {
            var player1 = NAudioEngine.Instance;
            var player2 = NAudioEngine.Instance;
            Assert.AreSame(player1, player2);
        }

        [TestMethod]
        public void NAudioEnginePlayLocalMp3FileTest()
        {
            player.Play();
            Assert.IsTrue(player.IsPlaying);
        }

        [TestMethod]
        public void NAudioEnginePauseTest()
        {
            if (!player.IsPlaying)
            {
                player.Play();
            }
            player.Pause();
            Assert.IsFalse(player.IsPlaying);
        }
    }
}
