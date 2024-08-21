using RSABot.Helpers;
using Newtonsoft.Json;

namespace RSABot.Models
{
    public class Errors : IOrderResponse
    {
        [JsonConverter(typeof(JsonArrayConvert<string>))]
        public List<string> error { get; set; }
    }

}
