using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RSABot.Helpers;

namespace RSABot.Models
{
    public class Profile
    {
        public string id { get; set; }
        public string name { get; set; }

        //[JsonConverter(typeof(JsonArrayConvert<Account>))]
        public List<Account> account { get; set; }
    }
}
