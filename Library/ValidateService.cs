using RSABot.Abstracts;
using Newtonsoft.Json;
using RSABot.Models;

namespace RSABot.Library
{
    public class ValidateService : IValidateService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public ValidateService(HttpClient httpClient, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://6n2bf2zta1.execute-api.us-west-1.amazonaws.com/Prod/");
        }

        public async Task<bool> ValidateLicense()
        {
            if (_tokenService.GetLicenseKey() == null) return false;
            string endpoint = $"ValidateMembership?key={_tokenService.GetLicenseKey()}";

            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error " + response);
            }

            string content = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? keys = JsonConvert.DeserializeObject<Dictionary<string, object>>(content.Replace("\"null\"", "null"));
            if (keys == null)
            {
                return false;
            }
            return Convert.ToBoolean(keys["valid"]);
        }
    }
}
