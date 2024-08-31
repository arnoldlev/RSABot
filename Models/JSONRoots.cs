using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class JSONRoots
    {
        public partial class ProfileRoot
        {
            public Profile profile { get; set; }
        }

        public partial class PositionsRoot
        {
            public Positions positions { get; set; }
        }

        public partial class OrdersRoot
        {
            public Orders orders { get; set; }
        }

        public partial class QuotesRoot
        {
            public Quotes quotes { get; set; }
        }

        public partial class PlaceOrderRoot
        {
            public PlaceOrder order { get; set; }
        }

        public partial class ErrorRoot
        {
            public Errors errors { get; set; }
        }

        public partial class BalanceRoot
        {
            public Balance balances { get; set; }
        }

    }

}
