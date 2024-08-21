using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class Errors : IOrderResponse
    {
        public List<string> error { get; set; }
    }

}
