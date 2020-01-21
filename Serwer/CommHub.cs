using LibVLCSharp.Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Owin;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Serwer
{
    [HubName("CommHub")]
    public class CommHub:Hub
    {
        VLCControl outputplayer = new VLCControl();
        public void RequestConnectionsList()            //potem przyjmij string na https
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            List<string> output = db.Table<VideoSources>().Select(p => p.Name).ToList();    //Pobiera z bazy listę nazw kamerek.
            Clients.Client(Context.ConnectionId).CamsInfo(output);
        }
        public void RequestOutStream(string requestedName)
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            string input = db.Table<VideoSources>().FirstOrDefault(p => p.Name == "test").Address;
            db.Close();
            var rtsp = new Media(MainWindow._libvlc, input, FromType.FromLocation);
            rtsp.AddOption(VLCControl.GetConfigurationString());
            Application.Current.Dispatcher.Invoke(() => 
            {
                var mainwindow = (MainWindow)Application.Current.MainWindow;
                mainwindow.outputMediaPlayer.Stop();
                //mainwindow.outputMediaPlayer.Play(rtsp);
                mainwindow.outputMediaPlayer.Play(new Media(MainWindow._libvlc, "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov", FromType.FromLocation));
            });
        }
    }
}
