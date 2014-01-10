using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Linq;

namespace FFXIV.Tools.AlertVoice
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        ffxivlib.Chatlog chatlog;
        Utility.TextTalker talker;

        public MainWindow()
        {
            InitializeComponent();

            this.talker = new Utility.TextTalker();

            this.DataContext = new Model.AlertVoiceModel()
            {
                Text = "ここにキーとなるアクション名を入力します。"
            };

            // FFXIVLIBに使用する言語を指定する
            Constants.ResourceParser.RESOURCES_LANGUAGE = "ja";

            // タイマーの設定（デフォルト100ミリ秒でログ更新）
            this.timer = new DispatcherTimer(DispatcherPriority.Normal);
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += this.ChatLogAnalyze_Tick;
        }

        /// <summary>
        /// 実行から1秒以内のログを取得する（タイマーが100ミリ秒なので過去1秒以内で十分）
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ffxivlib.Chatlog.Entry> GetChatLogLines()
        {
            if (null == chatlog)
            {
                chatlog = new ffxivlib.FFXIVLIB().GetChatlog();
            }

            if (chatlog.IsNewLine())
            {
                return chatlog.GetChatLogLines().Where(r => r.Timestamp > DateTime.Now.AddSeconds(-1));
            }

            return new List<ffxivlib.Chatlog.Entry>();
        }

        private void ChatLogAnalyze_Tick(object sender, EventArgs e)
        {
            var source = (Model.AlertVoiceModel)this.DataContext;

            string debugString = string.Empty;

            try
            {
                // 前回から増えたログがあれば抽出する
                if (!string.IsNullOrEmpty(source.Text))
                {
                    var keyActions = source.Text.Split('\n');

                    // 抽出したログ中にキーとなる文言が含まれていればアラート（音声出力）
                    foreach (var entry in this.GetChatLogLines())
                    {
                        var parser = new Log.LogParser(entry);

                        talker.TalkByDefaultVoice(parser.GetHitAction(keyActions));

                        debugString += parser.Log.Text + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                debugString = ex.Message;
            }

            source.DebugText = debugString;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            this.timer.Start();
            this.txtInputAction.IsHitTestVisible = false;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
            this.txtInputAction.IsHitTestVisible = true;
        }

        private void WindowBackground_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            this.Left += e.HorizontalChange;
            this.Top += e.VerticalChange;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
