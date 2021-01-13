using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroHue.Models;

namespace ZeroHue.Services
{
    public interface ILightRepository
    {
        Task<IDictionary<string, HueLight>> SelectAll();
        Task<HueLight> SelectById(int id);
        Task<bool> UpdateState(int id, LightState state);
    }
}
