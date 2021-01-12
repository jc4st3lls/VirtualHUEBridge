using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DiscoveryForAlexa
{
    public class UPNPServices
    {
        private static string ResponseMessage = "" +
            "HTTP/1.1 200 OK\r\n" +
            $"HOST: {AppSet.BroadcastIP}:{AppSet.BroadcastPort}\r\n" +
            "EXT:\r\n" +
            "CACHE-CONTROL: max-age=100\r\n" +
            $"LOCATION: http://{AppSet.FrontendIP}:{AppSet.FrontendPort}/description.xml\r\n" +
            "SERVER: Linux/3.14.0 UPnP/1.0 IpBridge/1.20.0\r\n" +
            $"hue-bridgeid: {AppSet.Huebridgeid}\r\n";

        private static string NotifyMessage = "" +
            "NOTIFY * HTTP/1.1\r\n" +
            $"HOST: {AppSet.BroadcastIP}:{AppSet.BroadcastPort}\r\n" +
            "CACHE-CONTROL: max-age=100\r\n" +
            $"LOCATION: http://{AppSet.FrontendIP}:{AppSet.FrontendPort}/description.xml\r\n" +
            "SERVER: Linux/3.14.0 UPnP/1.0 IpBridge/1.20.0\r\n" +
            "NTS: ssdp:alive\r\n" +
            $"hue-bridgeid: {AppSet.Huebridgeid}\r\n";

        private static string[] ST = new[]
        {
            "ST: upnp:rootdevice\r\n",
            $"ST: uuid:2f402f80-da50-11e1-9b23-{AppSet.FrontendIP}\r\n",
            "ST: urn:schemas-upnp-org:device:basic:1\r\n"
        };
        private static string[] NT = new[]
        {
            "NT: upnp:rootdevice\r\n",
            $"NT: uuid:2f402f80-da50-11e1-9b23-{AppSet.FrontendIP}\r\n",
            "NT: urn:schemas-upnp-org:device:basic:1\r\n"
        };
        private static string[] USN = new[]
        {
            $"USN: uuid:2f402f80-da50-11e1-9b23-{AppSet.FrontendIP}::upnp:rootdevice\r\n",
            $"USN: uuid:2f402f80-da50-11e1-9b23-{AppSet.FrontendIP}\r\n",
            $"USN: uuid:2f402f80-da50-11e1-9b23-{AppSet.FrontendIP}\r\n"
        };


        private static bool broadcaststarted = false;

        public event EventHandler StateChanged;
        public delegate void OnStateChanged(object obj, MessageArgs message);


        public void SsdpSearch()
        {
            IPEndPoint localEndPoint = null;
            UdpClient listener = null;
            IPEndPoint groupEP = null;
            bool echosearchdetected = false;
            try
            {


                localEndPoint = new IPEndPoint(IPAddress.Any, int.Parse(AppSet.BroadcastPort));
                listener = new UdpClient();
                listener.Client.Bind(localEndPoint);
                listener.JoinMulticastGroup(IPAddress.Parse(AppSet.BroadcastIP), IPAddress.Parse(AppSet.FrontendIP));

                groupEP = new IPEndPoint(IPAddress.Any, 0);

                long timeout = DateTime.Now.AddSeconds(30).Ticks;


                while (!echosearchdetected)
                {

                    var bytes = listener.Receive(ref groupEP);
                    var msgreveived = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    var msg = $"Received broadcast from {groupEP.ToString()} :\n {msgreveived}\n";



                    var sourceIP = groupEP.ToString().Split(":")[0];
                    var sourcePort = groupEP.ToString().Split(":")[1];

                    IPEndPoint source_ip = new IPEndPoint(IPAddress.Parse(sourceIP), int.Parse(sourcePort));

                    if (msgreveived.Contains("M-SEARCH * HTTP/1.1") && sourceIP.Equals(AppSet.EchoIP))
                    {
                        if (msgreveived.Contains("ssdp:discover"))
                        {
                            msg = $"Received **Echo** broadcast :\n {msgreveived}\n";
                            StateChanged?.Invoke(this, new MessageArgs() { Message = msg });

                            Random random = new Random(5);
                            int miliseg = random.Next(1000, 10000);
                            int sleep = miliseg / 10;

                            System.Threading.Thread.Sleep(sleep);
                            for (int i = 0; i < 3; i++)
                            {
                                msg = $"{ResponseMessage}{ST[i]}{USN[i]}\r\n";

                                byte[] bytestosend = Encoding.UTF8.GetBytes(msg);
                                listener.Send(bytestosend, bytestosend.Length, source_ip);


                                StateChanged?.Invoke(this, new MessageArgs() { Message = $"Send:\n{msg}" });
                            }

                            SsdpBroadcast();

                            echosearchdetected = true;
                        }


                    }
                    else
                    {
                        StateChanged?.Invoke(this, new MessageArgs() { Message = msg });
                    }



                }


            }
            catch (Exception e)
            {

            }
            finally
            {

            }

        }


        private void SsdpBroadcast()
        {
            IPEndPoint localEndPoint = null;
            UdpClient listener = null;

            try
            {
                localEndPoint = new IPEndPoint(IPAddress.Parse(AppSet.FrontendIP), 0);
                listener = new UdpClient(AddressFamily.InterNetwork);
                listener.Client.Bind(localEndPoint);
                listener.Client.Ttl = 2;

                listener.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

                IPEndPoint dst_ip = new IPEndPoint(IPAddress.Parse(AppSet.BroadcastIP), int.Parse(AppSet.BroadcastPort));

                string msg = string.Empty;
                for (int i = 0; i < 3; i++)
                {
                    msg = $"{NotifyMessage}{NT[i]}{USN[i]}\r\n";
                    byte[] bytestosend = Encoding.UTF8.GetBytes(msg);
                    listener.Send(bytestosend, bytestosend.Length, dst_ip);
                    listener.Send(bytestosend, bytestosend.Length, dst_ip);

                    StateChanged?.Invoke(this, new MessageArgs() { Message = $"Send:\n{msg + msg}" });
                }



            }
            catch (Exception e)
            {

            }
            finally
            {

            }

        }
    }

    public class MessageArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
