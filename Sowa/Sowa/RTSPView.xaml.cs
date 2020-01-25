using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sowa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RTSPView : ContentPage
    {
        const string VIDEO_URL = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";
        const string VURL2 = "rtsp://192.168.8.116:1337/go.sdp";
        readonly LibVLC _libvlc;

        public RTSPView()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            Core.Initialize();
            _libvlc = new LibVLC();
        }

        protected override void OnAppearing()
        {
            var connectionIP = Preferences.Get("SrvAddress", "0.0.0.0");
            var connectionPort= Preferences.Get("SrvPort", "1515");
            var connectionURL = "rtsp://" + connectionIP 
                + ":" + connectionPort + "/go.sdp";
            base.OnAppearing();
            VideoView.MediaPlayer = new MediaPlayer(_libvlc);
            VideoView.MediaPlayer.Media=new Media
                (_libvlc, connectionURL, FromType.FromLocation);
            VideoView.MediaPlayer.Play();
            VideoView.MediaPlayer.Mute = true;
        }
        protected override void OnDisappearing() 
        {
            base.OnDisappearing();
            VideoView.MediaPlayer.Dispose();
        }
    }

}