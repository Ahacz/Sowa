using LibVLCSharp.Shared;
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

namespace Serwer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string VIDEO_URL = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";
        readonly LibVLC _libvlc;

        public MainWindow()
        {
            InitializeComponent();
            // this will load the native libvlc library (if needed, depending on the platform). 
            Core.Initialize();

            // instantiate the main libvlc object
            _libvlc = new LibVLC();
            var mediaOptions = new[]
            {
                ":sout=#rtp{sdp=rtsp://192.168.1.162:8008/test}",
                ":sout-keep"
            };
            VideoView.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc);
            VideoView.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL, FromType.FromLocation));
        }
    }
}
