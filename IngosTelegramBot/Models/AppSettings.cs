using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IngosTelegramBot.Models
{
    public class AppSettings
    {
        public static string Url { get; set; } = "https://ingostelegrambot.azurewebsites.net:443/{0}";

        public static string Name { get; set; } = "IngosTestBot";

        public static string Key { get; set; } = "487963642:AAG6gxZSvrO3U9jEFZlZP3mPuEWZZs1e_po";
    }
}