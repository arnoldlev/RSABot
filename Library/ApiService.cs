using RSABot.Abstracts;
using RSABot.Helpers;
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
        private readonly IAppConfig _appConfig;
        private DateTime _rateLimitResetTime;
        private int _remainingRequests;
        private int _rateLimit;
        public ApiService(ITokenService tokenService, HttpClient httpClient, IAppConfig config)
        {
            _tokenService = tokenService ?? throw new Exception("Token Service cannot be null");
            _httpClient = httpClient;
            _appConfig = config;

            _httpClient.BaseAddress = new Uri(_appConfig.GetTradierURL());
        }

        public async Task<string> GetAsync(string endpoint)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAPIToken());

            await CheckRateLimitAsync();
            var response = await _httpClient.GetAsync(endpoint);        
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error " + response);
            }

            var content = await response.Content.ReadAsStringAsync();

            if (response.Headers.TryGetValues("X-RateLimit-Limit", out var limitValues))
            {
                _rateLimit = int.Parse(limitValues.First());
            }

            if (response.Headers.TryGetValues("X-RateLimit-Remaining", out var remainingValues))
            {
                _remainingRequests = int.Parse(remainingValues.First());
            }

            if (response.Headers.TryGetValues("X-RateLimit-Reset", out var resetValues))
            {
                _rateLimitResetTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(resetValues.First())).DateTime;
            }

            return content.Replace("\"null\"", "null");
        }

        public async Task<string> PostAsync(string endpoint, Dictionary<string, string> body)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAPIToken());

            await CheckRateLimitAsync();
            var response = await _httpClient.PostAsync(endpoint, new FormUrlEncodedContent(body));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error " + response);
            }
            var content = await response.Content.ReadAsStringAsync();
            if (response.Headers.TryGetValues("X-RateLimit-Limit", out var limitValues))
            {
                _rateLimit = int.Parse(limitValues.First());
            }

            if (response.Headers.TryGetValues("X-RateLimit-Remaining", out var remainingValues))
            {
                _remainingRequests = int.Parse(remainingValues.First());
            }

            if (response.Headers.TryGetValues("X-RateLimit-Reset", out var resetValues))
            {
                _rateLimitResetTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(resetValues.First())).DateTime;
            }

            return content.Replace("\"null\"", "null");
        }

        private async Task CheckRateLimitAsync()
        {
            if (_remainingRequests <= 0)
            {
                // Calculate the delay until rate limit reset
                var delay = _rateLimitResetTime - DateTime.UtcNow;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay);
                }

                // Reset remaining requests count after waiting
                _remainingRequests = _rateLimit;
            }
        }

        private string? GetAPIToken()
        {
            return _appConfig.GetEnvironment() == Environments.PROD ? _tokenService.GetAPIKey() : _tokenService.GetSandBoxKey();
        }

    }
}
