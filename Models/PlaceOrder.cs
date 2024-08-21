using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class PlaceOrder : IOrderResponse
    {
        public int id { get; set; }
        public string status { get; set; }
        public string partner_id { get; set; }
    }
}
