using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace FFXIV.Tools.AlertVoice
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Utility.TextTalker talker;

        public MainWindow()
        {
            InitializeComponent();

            talker = new Utility.TextTalker();

            // FFXIVLIBに使用する言語を指定する
            Constants.ResourceParser.RESOURCES_LANGUAGE = "ja";

            // タイマーの設定（デフォルト100ミリ秒でターゲット情報取得）
            this.timer = new DispatcherTimer(DispatcherPriority.Normal);
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += this.ChatLogAnalyze_Tick;
            this.timer.Start();
        }

        private void ChatLogAnalyze_Tick(object sender, EventArgs e)
        {
            try
            {
                var chatlog = new ffxivlib.FFXIVLIB().GetChatlog();
                var talker = new Utility.TextTalker();

                if (chatlog.IsNewLine())
                {
                    // 前回から増えたログがあれば抽出する
                    var lines = chatlog.GetChatLogLines();

                    // 抽出したログ中にキーとなる文言が含まれていればアラート（音声出力）
                    foreach (var l in lines)
                    {
                        if (new Log.LogParser(l).ContainsAction(""))
                        {
                            // アラート！！
                            this.talker.TalkByDefaultVoice("");
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Timer_Start(object sender, MouseEventArgs e)
        {
            this.timer.Start();
        }

        private void Timer_Stop(object sender, MouseEventArgs e)
        {
            this.timer.Stop();
        }
    }
}
