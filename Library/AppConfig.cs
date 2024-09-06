using RSABot.Abstracts;
using RSABot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
