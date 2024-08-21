using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using RSABot.Abstracts;
using RSABot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RSABot.Models.JSONRoots;

namespace RSABot.Library
{
    public class MarketService : IMarketService
    {
        private readonly ApiService _apiService;

        public MarketService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IOrderResponse> PlaceEquityOrder(string accountNumber, string symbol, string amount, string type)
        {
            var data = new Dictionary<string, string>
            {
                { "account_id", accountNumber },
                { "class", "equity" },
                { "symbol", symbol },
                { "side", type },
                { "quantity", amount },
                { "type", "market" },
                { "duration", "day" }
            };

            var response = await _apiService.PostAsync($"accounts/{accountNumber}/orders", data);
            ErrorRoot? errorRoot = JsonConvert.DeserializeObject<ErrorRoot>(response);
            if (errorRoot?.errors != null)
            {
                return errorRoot.errors;
            }

            PlaceOrderRoot? rootClass = JsonConvert.DeserializeObject<PlaceOrderRoot>(response);
            if (rootClass == null || rootClass.order == null)
            {
                return new PlaceOrder();
            }
            return rootClass.order;
        }

        public async Task<Quotes> GetQuotes(string symbol)
        {
            var response = await _apiService.GetAsync($"markets/quotes?symbols={symbol}");
            QuotesRoot? rootClass = JsonConvert.DeserializeObject<QuotesRoot>(response);
            if (rootClass == null || rootClass.quotes == null)
            {
                return new Quotes();
            }

            return rootClass.quotes;
        }

        public async Task<Orders> GetOrders(string accountNumber)
        {
            var response = await _apiService.GetAsync($"accounts/{accountNumber}/orders");
            OrdersRoot? rootClass = JsonConvert.DeserializeObject<OrdersRoot>(response);
            if (rootClass == null || rootClass.orders == null)
            {
                return new Orders();
            }

            return rootClass.orders;
        }

        public async Task<Positions> GetPositions(string accountNumber)
        {
            var response = await _apiService.GetAsync($"accounts/{accountNumber}/positions");
            PositionsRoot? rootClass = JsonConvert.DeserializeObject<PositionsRoot>(response);
            if (rootClass == null || rootClass.positions == null)
            {
                return new Positions();
            }

            return rootClass.positions;
        }
    }
}
