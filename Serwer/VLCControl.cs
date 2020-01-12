using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    class VLCControl
    {
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
    }
}
