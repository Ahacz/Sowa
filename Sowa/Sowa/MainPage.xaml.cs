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
            //string url = @"http://192.168.8.116:1337/";
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            AddressEntry.Text = Preferences.Get("SrvAddress", "0.0.0.0:8080");
        }
        private async void RTSPButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RTSPView());
        }
        private void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            InitiateServerConnection();
            GetCamsNames();
        }

        private async void InitiateServerConnection()
        {
            string uri = "https://" + AddressEntry.Text;
            try
            {
                connection = new HubConnection(@uri);
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
