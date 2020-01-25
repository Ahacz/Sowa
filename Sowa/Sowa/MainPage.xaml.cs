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
                connection = new HubConnection(@uri);
                //connection = new HubConnection(@"https://192.168.0.108:8080");
                _hub = connection.CreateHubProxy("CommHub");
                await connection.Start();
                Preferences.Set("SrvAddress", AddressEntry.Text);
                await _hub.Invoke("RequestConnectionsList");
            }
            catch (Exception c)
            {
                Console.WriteLine(c.StackTrace);
                Console.WriteLine(c.Message);
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

        private async void Connectionslist_ItemSelected
            (object sender, SelectedItemChangedEventArgs e)
        {
            var but =  Connectionslist.SelectedItem.ToString();
            await _hub.Invoke("RequestOutStream", but);
            await Navigation.PushAsync(new RTSPView());
        }
    }
}
