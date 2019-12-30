using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    [HubName("CommHub")]
    public class CommHub:Hub
    {
        public void Tester()
        {
            AddAddressDialog addressDialog = new AddAddressDialog();
            addressDialog.ShowDialog();
        }
    }
}
