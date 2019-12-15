﻿using LibVLCSharp.Shared;
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
            //var databasePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VideoSources.db");
            //var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            //var db = new SQLiteConnection(databasePath);
            InitializeComponent();            // this will load the native libvlc library (if needed, depending on the platform). 
            Core.Initialize();                // instantiate the main libvlc object
            _libvlc = new LibVLC();
            //db.Insert(new VideoSources {Name="test", Address="1.1.1.1" });
            VideoView.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc);
            var rtsp1 = new Media(_libvlc, VIDEO_URL, FromType.FromLocation);       //Create a media object and then set its options to duplicate streams - 1 on display 2 as RTSP
            rtsp1.AddOption(":sout=#duplicate" +
                "{dst=display{noaudio}," +
                "dst=rtp{mux=ts,dst=192.168.0.110,port=8080,sdp=rtsp://192.168.0.110:8080/go.sdp}");
            VideoView.MediaPlayer.Play(rtsp1);
            //VideoView1.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc);
            //VideoView1.MediaPlayer.Mute=true;
            //VideoView1.MediaPlayer.Play(new Media(_libvlc, "rtsp://192.168.0.110:8080/go.sdp", FromType.FromLocation));
        }
        private void OnClickSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }
    }
}
