using RSABot.Abstracts;
using CredentialManagement;

namespace RSABot.Library
{
    public class TokenService : ITokenService
    {

        public const string ApiTokenName = "RSABotAPIToken";
        public const string LicenseName = "RSABotLicense";

        public string GetAPIKey()
        {
            using (var cred = new Credential())
            {
                cred.Target = ApiTokenName;
                if (cred.Load())
                {
                    return cred.Password;
                }
                return null;
            }
        }

        public string GetLicenseKey()
        {
            using (var cred = new Credential())
            {
                cred.Target = LicenseName;
                if (cred.Load())
                {
                    return cred.Password;
                }
                return null;
            }
        }

        public void SaveAPIKey(string token)
        {
            using (var cred = new Credential())
            {
                cred.Password = token;
                cred.Target = ApiTokenName;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }

        public void SaveLicenseKey(string license)
        {
            using (var cred = new Credential())
            {
                cred.Password = license;
                cred.Target = LicenseName;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }
    }
}
