using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;

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
        private async void ConnectButton_OnClicked(object sender, EventArgs e)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://192.168.0.112:1337") //Make sure that the route is the same with your configured route for your HUB
                .Build();
            await Connect();
        }
        async Task Connect()
        {
            await connection.StartAsync();
            await connection.InvokeAsync("Tester");
        }

    }
}
