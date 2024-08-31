using RSABot.Abstracts;
using CredentialManagement;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.ComponentModel.Design;

namespace RSABot.Library
{
    public class TokenService : ITokenService
    {
        private const string SandBoxTokenName = "RSABotSandboxToken";
        private const string ApiTokenName = "RSABotAPIToken";
        private const string LicenseName = "RSABotLicense";
        private static readonly string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RSABot");

        public TokenService()
        {
            Directory.CreateDirectory(appDataPath); // Ensure the directory exists
        }

        public string? GetSandBoxKey()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            else
            {
                string filePath = Path.Combine(appDataPath, SandBoxTokenName);
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
        }

        public string? GetAPIKey()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            else
            {
                string filePath = Path.Combine(appDataPath, ApiTokenName);
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
        }

        public string? GetLicenseKey()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            else
            {
                string filePath = Path.Combine(appDataPath, LicenseName);
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
        }

        public void SaveSandBoxKey(string token)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            else
            {
                string filePath = Path.Combine(appDataPath, SandBoxTokenName);
                File.WriteAllText(filePath, token);
            }
        }

        public void SaveAPIKey(string token)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            else
            {
                string filePath = Path.Combine(appDataPath, ApiTokenName);
                File.WriteAllText(filePath, token);
            }
        }

        public void SaveLicenseKey(string license)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            else
            {
                string filePath = Path.Combine(appDataPath, LicenseName);
                File.WriteAllText(filePath, license);
            }
        }

    }
}
