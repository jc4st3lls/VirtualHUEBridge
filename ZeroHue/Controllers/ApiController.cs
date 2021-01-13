using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZeroHue.extensions;
using ZeroHue.Models;
using ZeroHue.Services;
using ZeroHue.Services.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeroHue.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        private const string _success = "success";

        ILightService _lightservice;

        public ApiController(LightsMessageCenter lightmessageservice)
        {
            _lightservice = lightmessageservice.LightService;
        }

        [HttpGet("{username}/lights")]
        public async Task<ActionResult<IDictionary<string,HueLight>>> GetLigths(string username)
        {
            if (!PassSecurity(username)) return Unauthorized();

            var lights = await _lightservice.GetAll();

            return Ok(lights);

        }

    

        [HttpGet("{username}/lights/{id}")]
        public async Task<ActionResult<HueLight>> Get(string username,int id)
        {
            if (!PassSecurity(username)) return Unauthorized();
            var huelight = await _lightservice.Get(id);

            return Ok(huelight);
            
        }

        

        [HttpPost]
        public async Task<ActionResult<object[]>> Post([FromBody] Device device)
        {
            await Task.CompletedTask; //Per treure warning async
           

            if (device.Devicetype.Equals("Echo"))
            {
                var obj = new { success = new { username = AppSet.USERNAME } };               
                return Ok(new[] { obj });
            }


            return NotFound();
        }

        
        [HttpPut("{username}/lights/{id}/state")]
        public async Task<ActionResult> PutState(string username,int id, [FromBody] LightState value)
        {

            if (!PassSecurity(username)) return Unauthorized();

            IList<string> response = new List<string>();


            var changed = await _lightservice.SetState(id,value);

            var property = "on";

            var stateproperty = $"/lights/{id}/state/{property}";
            var statepropertyvalue = value.On.ToString().ToLower();

            string statepropertyjson = "{" + $" \"{_success}\": " + "{ " + $"\"{stateproperty}\": {statepropertyvalue}" + "} }";
            response.Add(statepropertyjson);
            if (value.Bri != 0)
            {
                property = "bri";
                stateproperty = $"/lights/{id}/state/{property}";
                statepropertyvalue = value.Bri.ToString();

                statepropertyjson = "{" + $" \"{_success}\": " + "{ " + $"\"{stateproperty}\": {statepropertyvalue}" + "} }";
                response.Add(statepropertyjson);
            }
            if (value.Ct.HasValue)
            {
                property = "ct";
                stateproperty = $"/lights/{id}/state/{property}";
                statepropertyvalue = value.Ct.Value.ToString();

                statepropertyjson = "{" + $" \"{_success}\": " + "{ " + $"\"{stateproperty}\": {statepropertyvalue}" + "} }";
                response.Add(statepropertyjson);

            }
            if (value.Hue.HasValue)
            {
                property = "hue";
                stateproperty = $"/lights/{id}/state/{property}";
                statepropertyvalue = value.Hue.Value.ToString();

                statepropertyjson = "{" + $" \"{_success}\": " + "{ " + $"\"{stateproperty}\": {statepropertyvalue}" + "} }";
                response.Add(statepropertyjson);

            }
            if (value.Sat.HasValue)
            {
                property = "sat";
                stateproperty = $"/lights/{id}/state/{property}";
                statepropertyvalue = value.Sat.Value.ToString();

                statepropertyjson = "{" + $" \"{_success}\": " + "{ " + $"\"{stateproperty}\": {statepropertyvalue}" + "} }";
                response.Add(statepropertyjson);

            }


            //response[0] = "{ \"success\": { \"/lights/1/state/bri\": 200 } }";
            //response[1] = @"{ ""success"": { ""/lights/1/state/on"": true } }";
            //response[2] = @"{ ""success"": { ""/lights/1/state/hue"": 200 } }";


            string json = $"[{string.Join(",", response)}]";


            System.Text.Json.JsonDocument jdoc = System.Text.Json.JsonDocument.Parse(json);

            return Ok(jdoc.RootElement);
          


        }

        // Bad Security!!
        private bool PassSecurity(string username)
        {
            return username.Equals(AppSet.USERNAME)
                || username.Equals(AppSet.VIEWUSERNAME);
        }

    }
}
