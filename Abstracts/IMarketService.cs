using RSABot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Abstracts
{
    public interface IMarketService
    {
        Task<Positions> GetPositions(string accountNumber);
        Task<Orders> GetOrders(string accountNumber);
        Task<Quotes> GetQuotes(string symbol);
        Task<IOrderResponse> PlaceEquityOrder(string accountNumber, string symbol, string amount, string type);
    }
}
