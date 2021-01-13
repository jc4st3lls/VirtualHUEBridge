using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroHue.Models;

namespace ZeroHue.Services
{
    public class LightService : ILightService
    {
        ILightRepository _repository;

        public LightService(ILightRepository repository)
        {
            _repository = repository;
        }

        public event EventHandler StateChanged;
        public delegate void OnStateChanged(object obj, StateChangeArgs state);

        public async Task<HueLight> Get(int id)
        {
            return await _repository.SelectById(id);
        }

        public async Task<IDictionary<string, HueLight>> GetAll()
        {
            return await _repository.SelectAll();
        }

        public async Task<bool> SetState(int id, LightState state)
        {
            var ret= await _repository.UpdateState(id, state);

            StateChanged?.Invoke(this, new StateChangeArgs() { Id=id.ToString(), IsOn=state.On });


            return ret;
        }
    }

    public class StateChangeArgs : EventArgs
    {
        public string Id { get; set; }
        public bool IsOn { get; set; }
    }
}