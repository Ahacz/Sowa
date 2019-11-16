using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sowa
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
        private async void RTSPButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RTSPView());
        }
    }
}
