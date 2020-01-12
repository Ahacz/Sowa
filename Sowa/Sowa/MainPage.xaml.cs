using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sowa
{
    public partial class MainPage : ContentPage
    {
        private HubConnection connection;
        public MainPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
        private async void RTSPButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RTSPView());
        }
        private void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            IHubProxy _hub;
            string url = @"http://192.168.8.116:1337/";
            connection = new HubConnection(url);
            var writer = new DebugTextWriter();
            connection.TraceWriter = writer;
            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = Console.Out;
            _hub = connection.CreateHubProxy("CommHub");
            connection.Start().Wait();
            connection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);
            _hub.Invoke("RequestOutStream","kamerka");
        }
        private class DebugTextWriter : TextWriter
        {
            private StringBuilder buffer;

            public DebugTextWriter()
            {
                buffer = new StringBuilder();
            }

            public override void Write(char value)
            {
                switch (value)
                {
                    case '\n':
                        return;
                    case '\r':
                        Debug.WriteLine(buffer.ToString());
                        buffer.Clear();
                        return;
                    default:
                        buffer.Append(value);
                        break;
                }
            }

            public override void Write(string value)
            {
                Debug.WriteLine(value);

            }
            #region implemented abstract members of TextWriter
            public override Encoding Encoding
            {
                get { throw new NotImplementedException(); }
            }
            #endregion
        }
    }
}
