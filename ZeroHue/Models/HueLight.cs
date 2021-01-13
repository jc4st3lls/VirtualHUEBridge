using System;
using System.Text.Json.Serialization;

namespace ZeroHue.Models
{
    public class HueLight
    {
        [JsonPropertyName("state")]
        public LightState State { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("modelid")]
        public string Modelid { get; set; }

        [JsonPropertyName("manufacturername")]
        public string Manufacturername { get; set; }

        [JsonPropertyName("uniqueid")]
        public string Uniqueid { get; set; }

        [JsonPropertyName("swversion")]
        public string Swversion { get; set; }

        [JsonPropertyName("swconfigid")]
        public string Swconfigid { get; set; }

        [JsonPropertyName("productid")]
        public string Productid { get; set; }

        [JsonPropertyName("capabilities")]
        public Capabilities Capabilities { get; set; }

        [JsonPropertyName("config")]
        public Config Config { get; set; }

        [JsonPropertyName("productname")]
        public string Productname { get; set; }

        [JsonPropertyName("swupdate")]
        public Swupdate Swupdate { get; set; }

    }


    public partial class Capabilities
    {
        [JsonPropertyName("certified")]
        public bool Certified { get; set; }

        [JsonPropertyName("control")]
        public Control Control { get; set; }

        [JsonPropertyName("streaming")]
        public Streaming Streaming { get; set; }
    }

    public partial class Control
    {
        [JsonPropertyName("ct")]
        public Ct Ct { get; set; }

        [JsonPropertyName("maxlumen")]
        public long Maxlumen { get; set; }

        [JsonPropertyName("mindimlevel")]
        public long Mindimlevel { get; set; }
    }

    public partial class Ct
    {
        [JsonPropertyName("max")]
        public long Max { get; set; }

        [JsonPropertyName("min")]
        public long Min { get; set; }
    }

    public partial class Streaming
    {
        [JsonPropertyName("proxy")]
        public bool Proxy { get; set; }

        [JsonPropertyName("renderer")]
        public bool Renderer { get; set; }
    }

    public partial class Config
    {
        [JsonPropertyName("archetype")]
        public string Archetype { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }

        [JsonPropertyName("function")]
        public string Function { get; set; }

        [JsonPropertyName("startup")]
        public Startup Startup { get; set; }
    }

    public partial class Startup
    {
        [JsonPropertyName("configured")]
        public bool Configured { get; set; }

        [JsonPropertyName("customsettings")]
        public Customsettings Customsettings { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }
    }

    public partial class Customsettings
    {
        [JsonPropertyName("bri")]
        public long Bri { get; set; }

        [JsonPropertyName("ct")]
        public long Ct { get; set; }
    }

    public partial class LightState
    {
        [JsonPropertyName("on")]
        public bool On { get; set; }

        [JsonPropertyName("bri")]
        public long Bri { get; set; }

        [JsonPropertyName("alert")]
        public string Alert { get; set; }

        [JsonPropertyName("reachable")]
        public bool Reachable { get; set; }

        [JsonPropertyName("hue")]
        public long? Hue { get; set; }

        [JsonPropertyName("sat")]
        public long? Sat { get; set; }

        [JsonPropertyName("effect")]
        public string Effect { get; set; }

        [JsonPropertyName("xy")]
        public double[] Xy { get; set; }

        [JsonPropertyName("ct")]
        public long? Ct { get; set; }

        [JsonPropertyName("colormode")]
        public string Colormode { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }
    }

    public partial class Swupdate
    {
        [JsonPropertyName("lastinstall")]
        public DateTimeOffset Lastinstall { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
