using RSABot.Abstracts;
using RSABot.Models;
using Newtonsoft.Json;
using static RSABot.Models.JSONRoots;

namespace RSABot.Library
{
    public class ProfileService : IProfileService
    {

        private readonly ApiService _apiService;

        public ProfileService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Balance?> GetBalance(string accountNumber)
        {
            var response = await _apiService.GetAsync($"accounts/{accountNumber}/balances");
            BalanceRoot? rootClass = JsonConvert.DeserializeObject<BalanceRoot>(response);
            return (rootClass == null) ? null : rootClass.balances;
        }

    public async Task<Profile?> GetProfile()
        {
            try
            {
                var response = await _apiService.GetAsync("user/profile");

                ProfileRoot? rootClass = JsonConvert.DeserializeObject<ProfileRoot>(response);
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
