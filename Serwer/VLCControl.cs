using LibVLCSharp.Shared;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    class VLCControl
    {
        public Media rtsp;
        private const string VIDEO_URL = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";
        private MediaPlayer player;
        public static string GetConfigurationString()       //Metoda generująca konfiguracje dla odtwarzacza VLC
        {
            string address = Properties.Settings.Default.LocalAddress;
            string port = Properties.Settings.Default.LocalPort;
            string result=
                    ":sout=#duplicate" +
                    "{dst=display{noaudio}," +
                    "dst=rtp{mux=ts,dst=" + address +
                    ",port=" + port + ",sdp=rtsp://" + address + ":" + port + "/go.sdp}";
             return result;
        }
        public void playSelected(string inputAddress)
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            string input = db.Table<VideoSources>().FirstOrDefault(p => p.Name == "test").Address;
            db.Close();
            var rtsp = new Media(MainWindow._libvlc, input, FromType.FromLocation);
            rtsp.AddOption(VLCControl.GetConfigurationString());
            player.Stop();
            player.Play(new Media(MainWindow._libvlc, VIDEO_URL, FromType.FromLocation));
        }
    }
}
