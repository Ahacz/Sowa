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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sowa
{
    public partial class MainPage : ContentPage
    {
        private HubConnection connection;
        private IHubProxy _hub;
        private string srvAddress;
        public MainPage()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            //string url = @"http://192.168.8.116:1337/";
            srvAddress = Preferences.Get("SrvAddress","0.0.0.0:8080");
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
        private async void RTSPButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RTSPView());
        }
        private async void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            Preferences.Set("SrvAddress", srvAddress);
            connection = new HubConnection(srvAddress);
            var writer = new DebugTextWriter();
            connection.TraceWriter = writer;
            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = Console.Out;
            _hub = connection.CreateHubProxy("CommHub");
            await connection.Start();
            connection.Error += ex => Console.WriteLine("SignalR error: {0}", ex.Message);
            await _hub.Invoke("RequestConnectionsList");
            _hub.On<List<string>>("CamsInfo", namelist =>
            {
                Device.BeginInvokeOnMainThread(()=> UpdateLayout(namelist));

            });
        }
        private void UpdateLayout(List<string> input)
        {
            Connectionslist.ItemsSource = input;
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
        private void Connectionslist_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var but =  Connectionslist.SelectedItem.ToString();
            _hub.Invoke("RequestOutStream", but);
        }
    }
}
