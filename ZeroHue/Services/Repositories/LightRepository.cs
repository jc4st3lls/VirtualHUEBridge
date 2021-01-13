using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroHue.Models;

namespace ZeroHue.Services.Repositories
{
    public class LightRepository:ILightRepository
    {

        // POC: Memory Db
        // Using asign methods for apply with real DB
        private static ConcurrentDictionary<string, HueLight> Db => AppSet.LightsDatabase;

        public LightRepository()
        {
            
        }

        public async Task<IDictionary<string, HueLight>> SelectAll()
        {
            await Task.CompletedTask; // for earse async warninsgs
            return new Dictionary<string,HueLight>(Db.ToArray())
                .AsQueryable()
                .OrderBy(o => o.Key)
                .ToDictionary(t => t.Key, t => t.Value); 
        }

        public async Task<bool> UpdateState(int id, LightState state)
        {
            await Task.CompletedTask; // for earse async warninsgs
            Db.TryGetValue(id.ToString(), out var hue);
            if (hue != null)
            {
                lock (hue)
                {
                    hue.State.On=state.On;
                    if ( state.Bri != 0 ) hue.State.Bri = state.Bri;
                    if ( state.Ct.HasValue ) hue.State.Ct = state.Ct;
                    if ( state.Hue.HasValue ) hue.State.Hue = state.Hue;
                    if ( state.Sat.HasValue ) hue.State.Sat = state.Sat;
                }
                return true;
            }
            return false;
        }

        public async Task<HueLight> SelectById(int id)
        {
            await Task.CompletedTask; // for earse async warninsgs

            Db.TryGetValue(id.ToString(), out var hue);

            return hue;
        }
    }
}
