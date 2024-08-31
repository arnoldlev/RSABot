
using RSABot.Abstracts;
using RSABot.Helpers;
using RSABot.Models;
using System.Text;
using System.Text.Json;

namespace RSABot.Library
{
    public class DiscordService : IDiscordService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions JSON_OPTIONS = new() { PropertyNamingPolicy = new LowercaseNamingPolicy() };

    public DiscordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
         }

        public async Task PostMessage(List<OrderSummary> orders)
        {
            var url = "https://discord.com/api/webhooks/";
            var requestData = new
            {
                content = "[!] Stock Alert [!]",
                embeds = new[] { CreateEmbed(orders) }
            };

            string json = JsonSerializer.Serialize(requestData, JSON_OPTIONS);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, payload);

            string responseBody = await response.Content.ReadAsStringAsync();
        }


        public static Embed CreateEmbed(List<OrderSummary> orders)
        {
            var list = new List<EmbedField>();
            foreach (var order in orders)
            {
                list.Add(new EmbedField { Name = "Account Number", Value = order.AccountNumber, Inline = true });
                list.Add(new EmbedField { Name = "Status", Value = order.Status, Inline = true });
                list.Add(new EmbedField { Name = "Symbol", Value = order.Symbol, Inline = true });
                list.Add(new EmbedField { Name = "Quantity", Value = order.Amount, Inline = true });
                list.Add(new EmbedField { Name = "\u200B", Value = "\u200B", Inline = false });
            }

            return new Embed
            {
                Title = "RSABot - Order Details",
                Description = "Here are details of your sells",
                Url = "https://rsabot.com",
                Color = 65280,  // Green color in decimal
                Fields = list,
                Footer = new EmbedFooter
                {
                    Text = "Powered by RSABot"
                }
            };
        }

    }
}
