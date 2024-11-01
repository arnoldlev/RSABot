using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class Balance
    {
        public double option_short_value { get; set; }
        public double total_equity { get; set; }
        public string? account_number { get; set; }
        public string? account_type { get; set; }
        public double close_pl { get; set; }
        public double total_cash { get; set; }

    }
}
