using LibVLCSharp.Shared;
using SQLite;
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
using Microsoft.Owin.Hosting;

namespace Serwer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string VIDEO_URL = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";
        //const string VIDEO_URL = "rtsp://test:lolki5@192.168.8.240:88/videoMain";
        public static LibVLC _libvlc;
        public LibVLCSharp.Shared.MediaPlayer outputMediaPlayer;
        public Media outputmedia;
        public IDisposable SignalR { get; set; }
        public MainWindow()
        {
            //private HubConnection connection;
            /*string url = @"http://192.168.8.116:1337/";
            WebApp.Start<Startup>(url);
            //WebApp.Start<Startup>("https://192.168.8.116:8082");*/
            InitializeComponent();            // this will load the native libvlc library (if needed, depending on the platform). 
            Core.Initialize();                // instantiate the main libvlc object

            _libvlc = new LibVLC(new[]
            {
                "--verbose=2",
                "--file-logging",
                "--logfile=D:\\vlc-log2.txt"
            });
            outputMediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc);
            //CommHub hub = new CommHub(outputMediaPlayer);
            VideoView.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc) { Mute = true };
            var rtsp1 = new Media(_libvlc, VIDEO_URL, FromType.FromLocation);       //Create a media object and then set its options to duplicate streams - 1 on display 2 as RTSP
                                                                                    //string requestedStream = "";
                                                                                    //var outputStream = new Media(_libvlc, requestedStream, FromType.FromLocation);

            //rtsp1.AddOption(VLCControl.GetConfigurationString());
            /*rtsp1.AddOption(":sout=#duplicate" +
                "{dst=display{noaudio}," +
                "dst=rtp{mux=ts,dst=192.168.0.112,port=8080,sdp=rtsp://192.168.0.112:8080/go.sdp}");*/
            VideoView.MediaPlayer.Play(rtsp1);
            VideoView1.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc) { Mute = true };
            VideoView1.MediaPlayer.Play(new Media(_libvlc, "rtsp://192.168.8.116:1337/go.sdp", FromType.FromLocation));
            //IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CommHub>(); Może się przydać

        }
       /* public static void ChangeStreams(Media input)
        {
            outputMediaPlayer.Stop();
            outputMediaPlayer.Play(input);
        }*/
        private void OnClickStartSrv(object sender, RoutedEventArgs e)
        {
            string url = @"http://" + Properties.Settings.Default.LocalAddress + ":" + Properties.Settings.Default.LocalPort;
            SignalR = WebApp.Start<Startup> (url);
            StartButton.IsEnabled = false;
        }
        private void OnClickStopSrv(object sender, RoutedEventArgs e)
        {
            SignalR.Dispose();
            StartButton.IsEnabled = true;
        }
        private void OnClickSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }
    }
}
