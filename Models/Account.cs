using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class Account
    {
        public string account_number { get; set; }
        public string classification { get; set; }
        public DateTime date_created { get; set; }
        public bool day_trader { get; set; }
        public int option_level { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public DateTime last_update_date { get; set; }
    }
}
