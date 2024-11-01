using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class Embed
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("color")]
        public int Color { get; set; }

        [JsonPropertyName("fields")]
        public List<EmbedField> Fields { get; set; }

        [JsonPropertyName("footer")]
        public EmbedFooter Footer { get; set; }
    }

    public class EmbedField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("inline")]
        public bool Inline { get; set; }
    }

    public class EmbedFooter
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }


}
