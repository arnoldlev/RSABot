using Microsoft.Extensions.DependencyInjection;
using RSABot.Abstracts;
using RSABot.Helpers;
using RSABot.Library;
using RSABot.Models;
using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace RSABot
{
    public class Program
    {
        public static Profile? ProfilesCache;

        public async static Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddHttpClient<ApiService>((client) =>
            {
                client.BaseAddress = new Uri("https://sandbox.tradier.com/v1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IMarketService, MarketService>();
            var serviceProvider = services.BuildServiceProvider();

            Profile p = await ValidateToken(serviceProvider);
            await Menu(p, serviceProvider);


            //var data = new Dictionary<string, string>
            //{
            //    { "account_id", "VA4926170" },
            //    { "class", "equity" },
            //    { "symbol", "T" },
            //    { "side", "buy" },
            //    { "quantity", "1"},
            //    { "type", "market" },
            //    { "duration", "day" }
            //};

            //var api = serviceProvider.GetService<ApiService>();
            //var response = await api.PostAsync("accounts/VA4926170/orders", data);
            //Console.WriteLine(response);

            // await QuoteHandler(serviceProvider, "TSLA");

            //Profile p = await ProfileHandler(serviceProvider);
            // await PositionHandler(serviceProvider, p);

        }

        public static async Task Menu(Profile p, IServiceProvider services)
        {
            string? input;

            do
            {
                Console.WriteLine($"----- [ RSA Bot ] -----");
                Console.WriteLine($"1. Print Profile and Account Information");
                Console.WriteLine($"2. Print Order Information");
                Console.WriteLine($"3. Print Position Information");
                Console.WriteLine($"4. Sell all your stocks on all accounts (!) Danger // Not reversable (!)");
                Console.WriteLine($"Q. Exit the application");
                Console.WriteLine($"Info. Get stock information by entering Symbol");
                Console.WriteLine($"Buy.  Buy on all accounts with format: B:<StockName>:<StockAmount>");
                Console.WriteLine($"Sell. Sell on all accounts with format: S:<StockName>:<StockAmount>\n");

                Console.Write("SELECTION: ");
                input = Console.ReadLine();
                Console.WriteLine();
                while (input == null || input.Length == 0)
                {
                    Console.WriteLine("ERROR: Bad Selection Made. Try Again (Or Q to exit)");
                    input = Console.ReadLine();
                }

                string[] args = input.Split(":");
                Console.WriteLine(args.Length);

                if (input.ToUpper().Equals("Q"))
                {
                    Console.WriteLine("\nGoodBye!");
                }
                else if (input.Equals("1"))
                {
                    ProfileHandler(p);
                }
                else if (input.Equals("2"))
                {
                    await OrdersHandler(services, p);
                }
                else if (input.Equals("3"))
                {
                    await PositionHandler(services, p);
                }
                else if (input.Equals("4"))
                {
                    await SellAll(services, p);
                }
                else if (args.Length == 1)
                {
                    await QuoteHandler(services, input);
                }
                else if (args.Length == 3)
                {
                    string type = args[0], stock = args[1], amount = args[2];
                    if (!type.Equals("B") && !type.Equals("S"))
                    {
                        Console.WriteLine($"\nError: {type} is not a valid option. B for Buy | S for Sell\n");
                        continue;
                    }
                    if (!Validators.IsInt(amount))
                    {
                        Console.WriteLine($"\nError: {amount} is not a amount. Numeric values only!\n");
                        continue;
                    }
                    if (stock == null || stock.Length == 0)
                    {
                        Console.WriteLine($"\nError: {stock ?? "empty" }is not a valid stock!\n");
                        continue;
                    }

                    await PlaceOrderHandler(services, p, stock, amount, type);

                }

            } while (!input.ToUpper().Equals("Q"));
        }

        public static async Task SellAll(IServiceProvider services, Profile p)
        {
            Console.WriteLine("---- [ Selling All.... ] ----");
            var marketService = services.GetRequiredService<IMarketService>();
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
                    await PlaceOrderHandler(services, p, e.symbol, e.quantity.ToString(), "S");
                }
            }
        }

        public static async Task PlaceOrderHandler(IServiceProvider services, Profile p, string symbol, string amount, string type)
        {
            var marketService = services.GetRequiredService<IMarketService>();
            foreach (Account acc in p.account)
            {
                IOrderResponse po = await marketService.PlaceEquityOrder(acc.account_number, symbol, amount, type.Equals("B") ? "buy" : "sell"); 
                if (po == null)
                {
                    Console.WriteLine($"Unable to process - {symbol}\n");
                    return;
                }

                if (po is Errors)
                {
                    Errors err = (Errors)po;
                    Console.WriteLine($" ---- [ Order Error for {acc.account_number} ] ---- ");
                    foreach(var e in err.error)
                    {
                        Console.WriteLine("\t* " + e);
                    }
                    Console.WriteLine();

                } 
                else
                {
                    PlaceOrder order = (PlaceOrder)po;
                    Console.WriteLine($" ---- [ Order for {acc.account_number} ] ---- ");
                    Console.WriteLine($"\t* Id: {order.id}");
                    Console.WriteLine($"\t* Status: {order.status}");
                    Console.WriteLine($"\t* Partner Id: {order.partner_id}\n");
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
            Console.WriteLine($"\t* Bid: {e.bid}\n");    
        }

        public static async Task OrdersHandler(IServiceProvider services, Profile p)
        {
            var marketService = services.GetRequiredService<IMarketService>();

            foreach (Account acc in p.account)
            {
                Orders o = await marketService.GetOrders(acc.account_number);
                if (o.order == null || o.order.Count == 0)
                {
                    Console.WriteLine($"No Orders Found on Account {acc.account_number}\n");
                    return;
                }

                Console.WriteLine($"---- [ Orders - {acc.account_number} ] ----");
                foreach (Order e in o.order)
                {
                    Console.WriteLine($"\t* Symbol: {e.symbol}");
                    Console.WriteLine($"\t* Side: {e.side}");
                    Console.WriteLine($"\t* Quantity: {e.quantity}");
                    Console.WriteLine($"\t* Status: {e.status}");
                    Console.WriteLine($"\t* Type: {e.type}");
                    Console.WriteLine($"\t* Class: {e.@class}\n");
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
                    Console.WriteLine($"\t* Symbol: {e.symbol}");
                    Console.WriteLine($"\t* Cost Basis: {e.cost_basis}");
                    Console.WriteLine($"\t* Quantity: {e.quantity}");
                    Console.WriteLine($"\t* Date Acquired: {e.date_acquired}");
                    Console.WriteLine($"\t* Id: {e.id}\n");
                }
            }
        }

        public static void ProfileHandler(Profile p)
        {
            Console.WriteLine("---- [ Profile ] ----");
            if (p.id == "-1")
            {
                Console.WriteLine("Error: No Profile Found");
            }
            else
            {
                Console.WriteLine($"(*) {p.id} | Name: {p.name} | # of Accounts: {p.account.Count} ");
                foreach (Account acc in p.account)
                {
                    Console.WriteLine($"\t* Account Number: {acc.account_number}");
                    Console.WriteLine($"\t* Classification: {acc.classification}");
                    Console.WriteLine($"\t* Date Created: {acc.date_created}");
                    Console.WriteLine($"\t* Status: {acc.status}\n");
                }
            }
        }

        public async static Task<Profile?> ValidateProfile(IServiceProvider services)
        {
            try
            {
                var profileService = services.GetRequiredService<IProfileService>();
                Profile p = await profileService.GetProfile();
                return p;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async static Task<Profile> ValidateToken(IServiceProvider services)
        {
            var tokens = services.GetService<ITokenService>() ?? throw new Exception("Error: Missing Token Service");

            if (tokens.GetAPIKey() == null)
            {
                Console.WriteLine("Enter your API Token: ");
                var input = Console.ReadLine() ?? "Null";
                tokens.SaveAPIKey(input);
            }
            var p = await ValidateProfile(services);

            while (p == null)
            {
                Console.WriteLine("Error: Bad API Token. Could not authenticate. Try Again: ");
                var input = Console.ReadLine() ?? "Null";
                tokens.SaveAPIKey(input);
                p = await ValidateProfile(services);
            }
            return p;
        }
        
    }
}
