using System;
namespace DiscoveryForAlexa
{
    internal static class AppSet
    {
        internal static string BroadcastIP { set; get; } = "239.255.255.250";
        internal static string BroadcastPort { set; get; } = "1900";

        internal static string FrontendIP { get; set; } = "192.168.1.46";
        internal static string FrontendPort { get; set; } = "80";

        internal static string EchoIP { get; set; } = "192.168.1.98";

        internal static string Huebridgeid { get; set; } = "";
        static AppSet()
        {
            var seed = FrontendIP.Split(".");
            seed[1] += "FFFE8";
            Huebridgeid = string.Join(".", seed);
        }
    }
}
