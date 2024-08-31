using RSABot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Abstracts
{
    public interface IDiscordService
    {
        Task PostMessage(List<OrderSummary> orders);
    }
}
