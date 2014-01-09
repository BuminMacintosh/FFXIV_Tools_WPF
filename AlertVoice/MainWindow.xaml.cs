using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
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
        string debugString = string.Empty;
        ffxivlib.Chatlog chatlog;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new Model.AlertVoiceModel()
            {
                Text = "ここにキーとなるアクション名を入力します。"
            };

            // FFXIVLIBに使用する言語を指定する
            Constants.ResourceParser.RESOURCES_LANGUAGE = "ja";

            // 最初に起動したときにチャットログのバッファをクリアする
            chatlog = new ffxivlib.FFXIVLIB().GetChatlog();
            if (chatlog.IsNewLine())
            {
                var lines = chatlog.GetChatLogLines();
            }

            // タイマーの設定（デフォルト100ミリ秒でターゲット情報取得）
            this.timer = new DispatcherTimer(DispatcherPriority.Normal);
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += this.ChatLogAnalyze_Tick;
        }

        private void ChatLogAnalyze_Tick(object sender, EventArgs e)
        {
            var source = (Model.AlertVoiceModel)this.DataContext;

            debugString = string.Empty;

            try
            {
                var keyActions = source.Text.Split('\n');

                if (chatlog.IsNewLine())
                {
                    // 前回から増えたログがあれば抽出する
                    var lines = chatlog.GetChatLogLines();

                    foreach (var l in lines)
                    {
                        debugString += l.Text + Environment.NewLine;
                    }

                    // 抽出したログ中にキーとなる文言が含まれていればアラート（音声出力）
                    foreach (var l in lines)
                    {
                        var hit = new Log.LogParser(l).GetHitAction(keyActions);

                        if (!string.IsNullOrEmpty(hit))
                        {
                            debugString += "＊＊＊＊＊＊＊＊＊＊＊＊" + hit + "＊＊＊＊＊＊＊＊＊＊＊＊" + Environment.NewLine;
                            // アラート！！
                            Task.Factory.StartNew(() => new Utility.TextTalker().TalkByDefaultVoice(hit));
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            source.DebugText = debugString;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            this.timer.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
        }
    }


}
