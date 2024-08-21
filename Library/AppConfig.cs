using RSABot.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Library
{
    public class AppConfig : IAppConfig
    {
        private readonly Environments _env;

        public AppConfig(Environments env)
        {
            _env = env;
        }
        public Environments GetEnvironment()
        {
            return _env;
        }

        public string GetTradierURL()
        {
            return _env == Environments.PROD ? "https://api.tradier.com/v1/" : "https://sandbox.tradier.com/v1/";
        }
    }
}
