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
            var db = new SQLiteConnection(MainWindow.dbPath);
            List<string> output = 
                db.Table<VideoSources>().Select(p => p.Name).ToList();    
            Clients.Client(Context.ConnectionId).CamsInfo(output);
            Clients.Client(Context.ConnectionId)
                .OutputPort(Properties.Settings.Default.LocalPort);
        }
        public void RequestOutStream(string requestedName)
        {
            var db = new SQLiteConnection(MainWindow.dbPath);
            string input = db.Table<VideoSources>()
                .FirstOrDefault(p => p.Name == requestedName).Address;
            db.Close();
            var rtsp = new Media(MainWindow._libvlc, input, FromType.FromLocation);
            rtsp.AddOption(VLCControl.GetConfigurationString());
            Application.Current.Dispatcher.Invoke(() => 
            {
                var mainwindow = (MainWindow)Application
                    .Current.MainWindow;
                mainwindow.outputMediaPlayer.Stop();
                mainwindow.outputMediaPlayer.Media=rtsp;
                mainwindow.outputMediaPlayer.Play();
            });
        }
    }
}
