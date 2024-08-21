using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class Quote
    {
        public string symbol { get; set; }
        public string description { get; set; }
        public string exch { get; set; }
        public string type { get; set; }
        public double last { get; set; }
        public double change { get; set; }
        public int volume { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public object close { get; set; }
        public double bid { get; set; }
        public double ask { get; set; }
        public double change_percentage { get; set; }
        public int average_volume { get; set; }
        public int last_volume { get; set; }
        public long trade_date { get; set; }
        public double prevclose { get; set; }
        public double week_52_high { get; set; }
        public double week_52_low { get; set; }
        public int bidsize { get; set; }
        public string bidexch { get; set; }
        public long bid_date { get; set; }
        public int asksize { get; set; }
        public string askexch { get; set; }
        public long ask_date { get; set; }
        public string root_symbols { get; set; }
    }

    public class Quotes
    {
        public Quote quote { get; set; }
    }
}
