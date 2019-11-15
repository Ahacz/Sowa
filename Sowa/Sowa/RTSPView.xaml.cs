﻿using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sowa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RTSPView : ContentPage
    {
        const string VIDEO_URL = "rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov";
        readonly LibVLC _libvlc;

        public RTSPView()
        {
            InitializeComponent();

            // this will load the native libvlc library (if needed, depending on the platform). 
            Core.Initialize();

            // instantiate the main libvlc object
            _libvlc = new LibVLC();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // create mediaplayer objects,
            // attach them to their respective VideoViews
            // create media objects and start playback

            VideoView0.MediaPlayer = new MediaPlayer(_libvlc);
            VideoView0.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL, Media.FromType.FromLocation));

            VideoView1.MediaPlayer = new MediaPlayer(_libvlc);
            VideoView1.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL, Media.FromType.FromLocation));

            VideoView2.MediaPlayer = new MediaPlayer(_libvlc);
            VideoView2.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL, Media.FromType.FromLocation));

            VideoView3.MediaPlayer = new MediaPlayer(_libvlc);
            VideoView3.MediaPlayer.Play(new Media(_libvlc, VIDEO_URL, Media.FromType.FromLocation));
        }
    }

}