using RSABot.Helpers;
using Newtonsoft.Json;

namespace RSABot.Models
{
    public class Position
    {
        public double cost_basis { get; set; }
        public DateTime date_acquired { get; set; }
        public int id { get; set; }
        public double quantity { get; set; }
        public string symbol { get; set; }
    }

    public class Positions
    {
        [JsonConverter(typeof(JsonArrayConvert<Position>))]
        public List<Position> position { get; set; }
    }

}
