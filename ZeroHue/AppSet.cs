using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using ZeroHue.Models;

namespace ZeroHue
{
    public static class AppSet
    {
        public const string PATH_FILES = "./Models/Files/";


        public const string USERNAME = "83b7780291a6ceffbe0bd049104df";
        public const string VIEWUSERNAME = "v83b77802v";

        public static string ApiIP { get; internal set; }
        public static string ApiPort { get; internal set; }

        public static string FrontendIP { get; internal set; }
        public static string FrontendPort { get; internal set; }
        public static string Huebridgeid { get; internal set; }




        public static ConcurrentDictionary<string,HueLight> LightsDatabase = null;
        static AppSet()
        {
            try
            {
                //Initialize Dbs
                var ligths = System.IO.File.ReadAllText($"{PATH_FILES}Db.json");
                var dictionary = JsonSerializer.Deserialize<IDictionary<string, HueLight>>(ligths);
                LightsDatabase = new ConcurrentDictionary<string, HueLight>(dictionary);
            }
            catch(Exception)
            {
                throw new Exception("Can't create the database.");
            }
          


        }

        internal static X509Certificate2 GetSelfSignedCertificate()
        {
            var password = Guid.NewGuid().ToString();
            var commonName = "zerowar";
            var rsaKeySize = 2048;
            var years = 5;
            var hashAlgorithm = HashAlgorithmName.SHA256;


            using (var rsa = RSA.Create(rsaKeySize))
            {

                var request = new CertificateRequest($"cn={commonName}", rsa, hashAlgorithm, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                  new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false)
                );
                request.CertificateExtensions.Add(
                  new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false)
                );

                var certificate = request.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(years));
                if (certificate != null)
                {

                    if (AppSet.IsWindows())
                    {

                        certificate.FriendlyName = commonName;
                    }



                    // Return the PFX exported version that contains the key
                    return new X509Certificate2(certificate.Export(X509ContentType.Pfx, password), password, X509KeyStorageFlags.MachineKeySet);
                }

            }
            return null;
        }

        internal static bool IsWindows() =>
       RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        internal static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        internal static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    }
}
