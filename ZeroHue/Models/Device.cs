using System;
using System.Text.Json.Serialization;

namespace ZeroHue.Models
{
    public class Device
    {
        [JsonPropertyName("devicetype")]
        public string Devicetype { get; set; } 
    }
}
