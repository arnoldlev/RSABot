using RSABot.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Library
{
    public class ApiService
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        public ApiService(ITokenService tokenService, HttpClient httpClient)
        {
            _tokenService = tokenService ?? throw new Exception("Token Service cannot be null");
            _httpClient = httpClient;
        }

        public async Task<string> GetAsync(string endpoint)
        {
            //_httpClient.BaseAddress = new Uri("");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetAPIKey());

            var response = await _httpClient.GetAsync(endpoint);        
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error " + response);
            }

            var content = await response.Content.ReadAsStringAsync();
            return content.Replace("\"null\"", "null");
        }

        public async Task<string> PostAsync(string endpoint, Dictionary<string, string> body)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetAPIKey());
            var response = await _httpClient.PostAsync(endpoint, new FormUrlEncodedContent(body));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error " + response);
            }

            var content = await response.Content.ReadAsStringAsync();
            return content.Replace("\"null\"", "null");
        }


    }
}
