using System;
using System.IO;
using FFXIVLogPaser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;

namespace ToolTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var lines = File.ReadLines("C:\\Users\\avamin\\Desktop\\FFXIV_SAMPLELOG.txt");
            var converter = new RepLogConverter();

            using (ShimsContext.Create())
            {
                foreach (var l in lines)
                {
                    if (l.Contains("new log lines")) continue;

                    ffxivlib.Fakes.ShimChatlog.ShimEntry e = new ffxivlib.Fakes.ShimChatlog.ShimEntry();

                    e.CodeGet = () =>
                    {
                        try
                        {
                            return l.Substring(20, 4);
                        }
                        catch
                        {
                            return "0000";
                        }
                    };
                    e.TimestampGet = () =>
                    {
                        try
                        {
                            return DateTime.Parse(l.Substring(0, 19));
                        }
                        catch
                        {
                            return DateTime.MinValue;
                        }
                    };
                    e.TextGet = () =>
                    {
                        try
                        {
                            return l.Substring(29);
                        }
                        catch
                        {
                            return "";
                        }
                    };

                    var r = converter.Convert(e);

                    if (string.IsNullOrEmpty(r.From) && string.IsNullOrEmpty(r.To)) continue;

                    System.Diagnostics.Debug.WriteLine("{0}->{1}, {2}{5}{6}, D:{3}, C:{4}, \t{7}",
                        r.From, r.To, r.Action, r.Damage, r.Cure,
                        (r.IsParent) ? ":親"
                        : (r.IsChild) ? ":子": "",
                        (r.ActionStart) ? ":開始"
                        : (r.ActionEnd) ? ":終了" : "", l);
                }
            }
        }
    }
}
