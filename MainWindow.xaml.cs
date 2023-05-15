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

namespace _5thMeet
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        TimeSpan TimePosition;
        DispatcherTimer timer = null;
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var fd = new Microsoft.Win32.OpenFileDialog();           
            fd.Filter = "音訊檔案(*.mp3,*.3gp,*.wma)|*.mp3; *.3gp; *.wma|影片檔案(*.mp4, *.avi, *.mpeg, *.wmv)|*.mp4; *.avi; *.mpeg; *.wmv|所有檔案(*.*)|*.*";

            fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fd.ShowDialog();
            string filename = fd.FileName;
            if (filename != "")
            {
                TxtFilePath.Text = filename;
                Uri u = new Uri(filename);
                MedShow.Source = u;
                MedShow.Volume = 0.5f;
                MedShow.LoadedBehavior = MediaState.Play;
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            MedShow.LoadedBehavior = MediaState.Play;
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            MedShow.LoadedBehavior = MediaState.Pause;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            MedShow.LoadedBehavior = MediaState.Stop;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SliVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MedShow.Volume = SliVolume.Value;
        }

        private void MedShow_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimePosition = MedShow.NaturalDuration.TimeSpan;
            SliProgress.Minimum = 0;
            SliProgress.Maximum = TimePosition.TotalMilliseconds;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            SliProgress.Value = MedShow.Position.TotalMilliseconds;
        }

        private void SliProgress_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int SliderValue = (int)SliProgress.Value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            MedShow.Position = ts;
        }
    }
}
