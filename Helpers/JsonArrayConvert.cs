using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RSABot.Helpers
{
    public class JsonArrayConvert<T> : JsonConverter
    {
        private readonly int _maxItems;
        public JsonArrayConvert()
        {
            _maxItems = 1000;
        }
        public JsonArrayConvert(int maxItems)
        {
            _maxItems = maxItems;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (token.Type == JTokenType.Array)
            {
                // If the token is an array, limit the number of items deserialized to _maxItems
                JArray array = (JArray)token;
                List<T> items = [];
                for (int i = 0; i < Math.Min(array.Count, _maxItems); i++)
                {
                    items.Add(array[i].ToObject<T>());
                }
                return items;
            }

            // If the token is not an array, deserialize it as a single item in a list
            return new List<T> { token.ToObject<T>() };
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<T> list = (List<T>)value;
            if (list.Count == 1)
            {
                value = list[0];
            }
            serializer.Serialize(writer, value);
        }

        public override bool CanWrite
        {
            get { return true; }
        }
    }
}
