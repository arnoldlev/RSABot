using RSABot.Helpers;
using Newtonsoft.Json;

namespace RSABot.Models
{
    public class Order
    {
        public int id { get; set; }
        public string type { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public double quantity { get; set; }
        public string status { get; set; }
        public string duration { get; set; }
        public double avg_fill_price { get; set; }
        public double exec_quantity { get; set; }
        public double last_fill_price { get; set; }
        public double last_fill_quantity { get; set; }
        public double remaining_quantity { get; set; }
        public DateTime create_date { get; set; }
        public DateTime transaction_date { get; set; }
        public string @class { get; set; }
    }

    public class Orders
    {
        [JsonConverter(typeof(JsonArrayConvert<Order>))]
        public List<Order> order { get; set; }
    }
}
