using System;

namespace DiscoveryForAlexa
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("****************************************");
            Console.WriteLine("Dicovery for Alexa                      ");
            Console.WriteLine("                                        ");
            Console.WriteLine("       by @jc4st3lls - 2021 - Catalunya ");
            Console.WriteLine("****************************************");

            Console.WriteLine();
            Console.WriteLine();


            Run();

            Console.WriteLine("Bye.");
        }

        private static void Run()
        {
            UPNPServices upn = new UPNPServices();
            upn.StateChanged += (o, m) =>
            {
                var msg = (MessageArgs)m;
                Console.WriteLine(msg.Message);
            };
            Console.WriteLine("Waiting for Echo broadcast");
            upn.SsdpSearch();

        }
    }
}
