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

        public async Task<Profile> GetProfile()
        {
            var response = await _apiService.GetAsync("user/profile");
            ProfileRoot rootClass = JsonConvert.DeserializeObject<ProfileRoot>(response);
            if (rootClass == null)
            {
                return new Profile() { id = "-1" };
            }

            return rootClass.profile;
        }
    }
}
