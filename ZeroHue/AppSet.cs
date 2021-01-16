using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using ZeroHue.Models;

namespace ZeroHue
{
    public static class AppSet
    {
        public const string PATH_FILES = "./Models/Files/";


        public const string USERNAME = "83b7780291a6ceffbe0bd049104df";
        public const string VIEWUSERNAME = "v83b77802v";

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
    }
}
