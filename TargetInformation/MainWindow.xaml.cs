using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FFXIV.Tools.TargetInformation
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            // FFXIVLIBに使用する言語を指定する
            Constants.ResourceParser.RESOURCES_LANGUAGE = "ja";

            // タイマーの設定（デフォルト100ミリ秒でターゲット情報取得）
            this.timer = new DispatcherTimer(DispatcherPriority.Normal);
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += this.UpdateInformation_Tick;
            this.timer.Start();
        }

        private void Timer_Start(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MainWindow_MouseLeave");
            this.timer.Start();
        }

        private void Timer_Stop(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MainWindow_MouseEnter");
            this.timer.Stop();
        }

        private void UpdateInformation_Tick(object sender, EventArgs e)
        {
            try
            {
                var ffxiv = new ffxivlib.FFXIVLIB();
                this.DataContext = new Model.TargetInformationModel(ffxiv.GetCurrentTarget(),
                                                                    ffxiv.GetPartyMemberInfo(0));
            }
            catch (Exception ex)
            {
                this.DataContext = new Model.TargetInformationModel(ex);
            }
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
