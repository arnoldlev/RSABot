using RSABot.Abstracts;
using CredentialManagement;

namespace RSABot.Library
{
    public class TokenService : ITokenService
    {

        public const string SandBoxTokenName = "RSABotSandboxToken";
        public const string ApiTokenName = "RSABotAPIToken";
        public const string LicenseName = "RSABotLicense";

        public string? GetSandBoxKey()
        {
            using (var cred = new Credential())
            {
                cred.Target = SandBoxTokenName;
                cred.Username = SandBoxTokenName;
                if (cred.Load())
                {
                    return cred.Password;
                }
                return null;
            }
        }

        public string? GetAPIKey()
        {
            using (var cred = new Credential())
            {
                cred.Target = ApiTokenName;
                cred.Username = ApiTokenName;
                if (cred.Load())
                {
                    return cred.Password;
                }
                return null;
            }
        }

        public string? GetLicenseKey()
        {
            using (var cred = new Credential())
            {
                cred.Target = LicenseName;
                cred.Username = LicenseName;
                if (cred.Load())
                {
                    return cred.Password;
                }
                return null;
            }
        }

        public void SaveSandBoxKey(string token)
        {
            using (var cred = new Credential())
            {
                cred.Target = SandBoxTokenName;
                cred.Username = SandBoxTokenName;
                cred.Password = token;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }

        public void SaveAPIKey(string token)
        {
            using (var cred = new Credential())
            {
                cred.Username = ApiTokenName;
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
                cred.Username = LicenseName;
                cred.Password = license;
                cred.Target = LicenseName;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }
    }
}
