using Microsoft.Extensions.DependencyInjection;
using RSABot.Abstracts;
using RSABot.Models;

namespace RSABot.Handlers
{
    public class MarketHandler
    {
        public static async Task SellAll(IServiceProvider services, Profile p)
        {
            Console.WriteLine("---- [ Selling All.... ] ----");
            var marketService = services.GetRequiredService<IMarketService>();

            if (!ConfirmOrder(true))
            {
                Console.WriteLine("(!) Action has been cancelled!\n");
                return;
            }

            foreach (Account acc in p.account)
            {
                Positions pos = await marketService.GetPositions(acc.account_number);
                if (pos.position == null || pos.position.Count == 0)
                {
                    Console.WriteLine($"No Positions Found on Account {acc.account_number}\n");
                    return;
                }

                foreach (Position e in pos.position)
                {
                    await PlaceOrderHandler(services, p, e.symbol, e.quantity.ToString(), "S", false);
                }
            }
        }


        public static async Task QuoteHandler(IServiceProvider services, string symbol)
        {
            var marketService = services.GetRequiredService<IMarketService>();
            Quotes q = await marketService.GetQuotes(symbol);
            if (q.quote == null)
            {
                Console.WriteLine($"No Quotes Found for {symbol}\n");
                return;
            }
            Quote e = q.quote;
            Console.WriteLine("---- [ Quotes ] ----");
            Console.WriteLine($"\t* Symbol: {e.symbol}");
            Console.WriteLine($"\t* Type: {e.type}");
            Console.WriteLine($"\t* Description: {e.description}");
            Console.WriteLine($"\t* Bid: {e.bid:C}\n");
        }

        public static async Task OrdersHandler(IServiceProvider services, Profile p)
        {
            var marketService = services.GetRequiredService<IMarketService>();
            foreach (Account acc in p.account)
            {
                Orders o = await marketService.GetOrders(acc.account_number);
                if (o.order == null || o.order.Count == 0)
                {
                    Console.WriteLine($"No Orders Found on All Accounts\n");
                    return;
                }

                Console.WriteLine($"---- [ Orders - {acc.account_number} ] ----");
                foreach (Order e in o.order)
                {
                    Console.WriteLine($"\t* Symbol: {e.symbol}");
                    Console.WriteLine($"\t* Side: {e.side}");
                    Console.WriteLine($"\t* Quantity: {e.quantity}");
                    Console.WriteLine($"\t* Status: {e.status}");
                    Console.WriteLine($"\t* Type: {e.type}\n");
                }
            }
        }

        public static async Task PositionHandler(IServiceProvider services, Profile p)
        {
            var marketService = services.GetRequiredService<IMarketService>();
            foreach (Account acc in p.account)
            {
                Positions pos = await marketService.GetPositions(acc.account_number);
                if (pos.position == null || pos.position.Count == 0)
                {
                    Console.WriteLine($"* No Positions Found on Account {acc.account_number}\n");
                    return;
                }

                Console.WriteLine($"---- [ Positions - {acc.account_number} ] ----");
                foreach (Position e in pos.position)
                {
                    
                    Quotes q = await marketService.GetQuotes(e.symbol);
                    if (q != null && q.quote != null)
                        Console.WriteLine($"\t* {e.quantity}x | {e.symbol} | Cost: {q.quote.bid:C}");
                    else 
                        Console.WriteLine($"\t* {e.quantity}x | {e.symbol} | Cost: Unknown");
                }
            }
        }

        public static bool ConfirmOrder(bool sellAll, string? symbol = null, string? amount = null, string? type = null, double? price = null)
        {
            string? confirm;
            if (sellAll)
            {
                Console.Write("Are you sure you want to ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("SELL ALL");
                Console.ResetColor();
                Console.Write(" of your stocks? (Y/N): ");
                confirm = Console.ReadLine();
            }
            else
            {
                Console.Write("Are you sure you want to ");

                Console.ForegroundColor = type.Equals("B") ? ConsoleColor.Green : ConsoleColor.Red;
                if (type.Equals("B"))
                    Console.Write("Buy");
                else
                    Console.Write("Sell");

                Console.ResetColor();
                if (amount.Equals("1"))
                    Console.Write($" {amount} share of {symbol} at {price:C}? (Y/N) ");
                else
                    Console.Write($" {amount} shares of {symbol} at {price:C}? (Y/N) ");
                confirm = Console.ReadLine();
            }

            return confirm != null && confirm.Equals("Y");
        }

        public static async Task<List<OrderSummary>> PlaceOrderHandler(IServiceProvider services, Profile p, string symbol, string amount, string type, bool confirm)
        {
            var totals = new List<OrderSummary>();

            var marketService = services.GetRequiredService<IMarketService>();
            Quotes q = await marketService.GetQuotes(symbol);
            if (q.quote == null)
            {
                Console.WriteLine($"Unable to process - Invalid Stock: {symbol}\n");
                return totals;
            }

            if (confirm)
            {
                if (!ConfirmOrder(false, symbol, amount, type, q.quote.bid))
                {
                    Console.WriteLine("(!) Action has been cancelled!\n");
                    return totals;
                }
            }

            foreach (Account acc in p.account)
            {
                IOrderResponse po = await marketService.PlaceEquityOrder(acc.account_number, symbol, amount, type.Equals("B") ? "buy" : "sell");
                if (po == null)
                {
                    Console.WriteLine($"Unable to process - {symbol}\n");
                    return totals;
                }

                if (po is Errors err)
                {
                    Console.WriteLine($" ---- [ Order Error for {acc.account_number} ] ---- ");
                    foreach (var e in err.error)
                    {
                        Console.WriteLine("\t* " + e);
                    }
                    Console.WriteLine();
                    totals.Add(new OrderSummary() { AccountNumber = acc.account_number, Symbol = symbol, Amount = amount, Status = "Failed" });
                }
                else
                {
                    PlaceOrder order = (PlaceOrder)po;
                    Console.WriteLine($" ---- [ Order for {acc.account_number} ] ---- ");
                    Console.WriteLine($"\t* Id: {order.id}");
                    Console.WriteLine($"\t* Status: {order.status}");
                    Console.WriteLine($"\t* Partner Id: {order.partner_id}\n");
                    totals.Add(new OrderSummary() { AccountNumber = acc.account_number, Symbol = symbol, Amount = amount, Status = "Success" });
                }

            }
            return totals;
        }

    }
}
