using RSABot.Abstracts;
using RSABot.Models;
using Newtonsoft.Json;
using static RSABot.Models.JSONRoots;
using RSABot.Helpers;

namespace RSABot.Library
{
    public class ProfileService : IProfileService
    {
        private readonly ApiService _apiService;
        private readonly IValidateService _validateService;

        public ProfileService(ApiService apiService, IValidateService validateService)
        {
            _apiService = apiService;
            _validateService = validateService;
        }

        public async Task<Balance?> GetBalance(string accountNumber)
        {
            var response = await _apiService.GetAsync($"accounts/{accountNumber}/balances");
            BalanceRoot? rootClass = JsonConvert.DeserializeObject<BalanceRoot>(response);
            return (rootClass == null) ? null : rootClass.balances;
        }

        public async Task<Profile?> GetProfile()
        {
            int limit = _validateService.GetLimit();
            var settings = new JsonSerializerSettings
            {
                Converters = [ new JsonArrayConvert<Account>(limit) ]
            };
            try
            {
                var response = await _apiService.GetAsync("user/profile");

                ProfileRoot? rootClass = JsonConvert.DeserializeObject<ProfileRoot>(response, settings);
                if (rootClass == null)
                {
                    return null;
                }

                return rootClass.profile;
            } catch (Exception)
            {
                return null;
            }
        }
    }
}
