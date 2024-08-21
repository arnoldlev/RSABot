using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RSABot
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Environments : short
    {
        [EnumMember(Value = "https://api.tradier.com/v1/")]
        PROD = 1,

        [EnumMember(Value = "https://sandbox.tradier.com/v1/")]
        SANDBOX = 2
    }
}
