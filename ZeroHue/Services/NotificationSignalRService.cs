using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ZeroHue.Services.Hubs;

namespace ZeroHue.Services
{
    public class NotificationSignalRService:INotificationService
    {

        IHubContext<HueLightsHub> _hub;
        public NotificationSignalRService(IHubContext<HueLightsHub> hub)
        {
            _hub = hub;
        }

        public async Task SendONState(string id, bool state)
        {
            await _hub.Clients.All.SendAsync("ReceiveONState", id, state);
        }
    }
}
