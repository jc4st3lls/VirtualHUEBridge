using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroHue.Models;

namespace ZeroHue.Services
{
    public interface ILightService
    {
        event EventHandler StateChanged;
        Task<IDictionary<string, HueLight>> GetAll();
        Task<HueLight> Get(int id);
        Task<bool> SetState(int id, LightState state);
    }
}
