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

        // GET: api/values
        [HttpGet("{username}/lights")]
        public async Task<ActionResult<IDictionary<string,HueLight>>> GetLigths(string username)
        {
            //var ligths = await System.IO.File.ReadAllTextAsync("./Models/huelights.json");
            //System.Text.Json.JsonDocument jdoc = System.Text.Json.JsonDocument.Parse(ligths);

            //return Ok(jdoc.RootElement);

            var lights = await _lightservice.GetAll();

            return Ok(lights);

        }

        // GET api/values/5
        [HttpGet("{username}/lights/{id}")]
        public async Task<ActionResult<HueLight>> Get(string username,int id)
        {
            //var ligth = await System.IO.File.ReadAllTextAsync($"./Models/light{id}.json");
            //System.Text.Json.JsonDocument jdoc = System.Text.Json.JsonDocument.Parse(ligth);

            //return Ok(jdoc.RootElement);

            var huelight = await _lightservice.Get(id);

            return Ok(huelight);
            
        }

        // POST api/values
        //[HttpPost]
        //public async Task<ActionResult> Post()
        //{

        //    //{"devicetype": "my_hue_app#iphone peter"}

        //    var reader = new StreamReader(Request.Body);
        //    var value = await reader.ReadToEndAsync();
        //    var requestData = System.Text.Json.JsonDocument.Parse(value);
        //    var root = requestData.RootElement;
        //    var request = root.Get("devicetype").Value;

        //    //{"devicetype": "Echo"}
        //    if (request.ValueEquals("Echo"))
        //    {
        //        var response= @"[{""success"":{""username"":""" + AppSet.USERNAME + @"""}}]";
        //        System.Text.Json.JsonDocument jdoc = System.Text.Json.JsonDocument.Parse(response);

        //        return Ok(jdoc.RootElement);
        //    }
        //    return NotFound();
        //}

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

        // PUT api/values/5
        [HttpPut("{username}/lights/{id}/state")]
        public async Task<ActionResult> PutState(string username,int id, [FromBody] LightState value)
        {
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
