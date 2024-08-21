using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Abstracts
{
    public interface ITokenService
    {
        void SaveAPIKey(string token);
        string GetAPIKey();
        void SaveLicenseKey(string license);
        string GetLicenseKey();
    }
}
