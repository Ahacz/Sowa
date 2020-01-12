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

namespace Serwer
{
    [HubName("CommHub")]
    public class CommHub:Hub
    {
        private const string VIDEO_URL = "rtsp://test:lolki5@192.168.8.240:88/videoMain";
        private MediaPlayer player;
        public CommHub()
        {
            //player = mainpl;
            /*var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            string input = db.Table<VideoSources>().FirstOrDefault(p => p.Name == "test").Address;
            db.Close();
            rtsp = new Media(_libvlc, input, FromType.FromLocation);
            string testst = VLCControl.GetConfigurationString();
            rtsp.AddOption(testst);*/

        }
        public void Tester()
        {
        }
        public void RequestOutStream(string requestedName)
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            string input = db.Table<VideoSources>().FirstOrDefault(p => p.Name == "test").Address;
            db.Close();
            Media rtsp = new Media(MainWindow._libvlc, input, FromType.FromLocation);
            rtsp.AddOption(VLCControl.GetConfigurationString());
            //MainWindow.outputMediaPlayer.Play(rtsp);
        }
    }
}
