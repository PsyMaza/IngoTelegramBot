using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IngosTelegramBot.Models
{
    public class User
    {
        public long ChatID { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
    }
}