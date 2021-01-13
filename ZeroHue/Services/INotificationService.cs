using System;
using System.Threading.Tasks;

namespace ZeroHue.Services
{
    public interface INotificationService
    {
        Task SendONState(string id, bool state);
    }
}
