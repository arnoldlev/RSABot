using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Abstracts
{
    public interface IAppConfig
    {
        public Environments GetEnvironment();
        public string GetTradierURL();
    }
}
