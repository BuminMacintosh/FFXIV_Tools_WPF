using FFXIV.Tools.AlertVoice.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToolTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TextTalker talker = new TextTalker();

            for (int i = 0; i < 5; i++)
            {
                talker.TalkByDefaultVoice("<pitch middle=\"5\">１．ピッチをあげて、こんにちは世界。</pitch>");

                talker.TalkByDefaultVoice("<pitch middle=\"-5\">２．ピッチをさげて、こんにちは世界。</pitch>");
            }

            talker.WaitUntilSpoken();
        }
    }
}
