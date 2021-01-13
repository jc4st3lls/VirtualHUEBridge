using System;
namespace ZeroHue.Services
{
    public class LightsMessageCenter
    {
        
        public LightsMessageCenter(ILightService lightService, INotificationService notification)
        {
            LightService = lightService;

            LightService.StateChanged += (obj, state) =>
            {
                var _state = (StateChangeArgs)state;
                notification.SendONState(_state.Id, _state.IsOn);

            };
        }

        public ILightService LightService { get; private set; }
        
    }
}
