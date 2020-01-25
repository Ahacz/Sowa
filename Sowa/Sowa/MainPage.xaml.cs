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
        private List<string> localNamesList;
        public MainPage()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            AddressEntry.Text = Preferences.Get("SrvAddress", "0.0.0.0");
        }
        private void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            InitiateServerConnection();
            GetCamsNames();
            GetOutputPort();
        }

        private async void InitiateServerConnection()
        {
            string uri = "https://" + AddressEntry.Text+":8080";
            try
            {
                //connection = new HubConnection(@uri);
                connection = new HubConnection(@"http://192.168.8.116:8080");
                var writer = new DebugTextWriter();
                connection.TraceWriter = writer;
                connection.TraceLevel = TraceLevels.All;

                _hub = connection.CreateHubProxy("CommHub");
                await connection.Start();
                Preferences.Set("SrvAddress", AddressEntry.Text);
                await _hub.Invoke("RequestConnectionsList");
            }
            catch (System.Net.Http.HttpRequestException)
            {
            }
        }
        private void GetCamsNames()
        {
            _hub.On<List<string>>("CamsInfo", namelist =>
            {
                localNamesList = namelist;
                Device.BeginInvokeOnMainThread(() 
                    => Connectionslist.ItemsSource = localNamesList);
            });
        }
        
        private void GetOutputPort()
        {
            _hub.On<string>("OutputPort", port =>
            {
                Preferences.Set("SrvPort", port);
            });
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
                        Console.WriteLine(buffer.ToString());
                        buffer.Clear();
                        return;
                    default:
                        buffer.Append(value);
                        break;
                }
            }

            public override void Write(string value)
            {
                Console.WriteLine(value);

            }
            #region implemented abstract members of TextWriter
            public override Encoding Encoding
            {
                get { throw new NotImplementedException(); }
            }
            #endregion
        }
        private async void Connectionslist_ItemSelected
            (object sender, SelectedItemChangedEventArgs e)
        {
            var but =  Connectionslist.SelectedItem.ToString();
            await _hub.Invoke("RequestOutStream", but);
            await Navigation.PushAsync(new RTSPView());
        }
    }
}
