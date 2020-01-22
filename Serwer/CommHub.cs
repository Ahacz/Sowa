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
        public void RequestConnectionsList() 
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            List<string> output = 
                db.Table<VideoSources>().Select(p => p.Name).ToList();    
            Clients.Client(Context.ConnectionId).CamsInfo(output);
        }
        public void RequestOutStream(string requestedName)
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            string input = db.Table<VideoSources>()
                .FirstOrDefault(p => p.Name == requestedName).Address;
            db.Close();
            var rtsp = new Media(MainWindow._libvlc, input, FromType.FromLocation);
            rtsp.AddOption(VLCControl.GetConfigurationString());
            Application.Current.Dispatcher.Invoke(() => 
            {
                var mainwindow = (MainWindow)Application.Current.MainWindow;
                mainwindow.outputMediaPlayer.Stop();
                mainwindow.outputMediaPlayer.Play(rtsp);
            });
        }
    }
}
