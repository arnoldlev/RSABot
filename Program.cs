using Microsoft.Extensions.DependencyInjection;
using RSABot.Abstracts;
using RSABot.Handlers;
using RSABot.Helpers;
using RSABot.Library;
using RSABot.Models;
using System.Globalization;
using System.Text;

namespace RSABot
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            // Get Prod or Sandbox
            Environments? env = SetEnvironment();
            if (env == null) return;

            // Inject Services
            var services = new ServiceCollection();
            services.AddSingleton<IAppConfig, AppConfig>(provider =>
            {
                return new AppConfig(env.Value);
            });
            services.AddSingleton<ITokenService, TokenService>();
            services.AddHttpClient<ApiService>();
            services.AddHttpClient<IValidateService, ValidateService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IMarketService, MarketService>();
            services.AddTransient<IDiscordService, DiscordService>();

            var serviceProvider = services.BuildServiceProvider();

            // Check License Key
            bool isLicensned = await AuthHandler.ValidateLicense(serviceProvider);
            if (!isLicensned) return;

            // Check API Token
            Profile? p = (env == Environments.PROD) ? await AuthHandler.ValidateAPIKey(serviceProvider) : await AuthHandler.ValidateSandboxKey(serviceProvider);
            if (p == null) return;

            // Display Menu
            await Menu(p, serviceProvider);
        }

        public static async Task Menu(Profile p, IServiceProvider services)
        {
            string? input;
            do
            {
                Console.WriteLine($"\n----- [ RSA Bot ] -----");
                Console.WriteLine($"1. Print Profile and Account Information");
                Console.WriteLine($"2. Print Order Information");
                Console.WriteLine($"3. Print Position Information");
                Console.WriteLine($"4. Sell all your stocks on all accounts (!) Danger // Not reversable (!)");
               // Console.WriteLine($"5. Enter Discord Channel Webhook URL");
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
                if (input.ToUpper().Equals("Q"))
                {
                    Console.WriteLine("\nGoodBye!");
                }
                else if (input.Equals("1"))
                {
                    await UserHandler.ProfileHandler(services, p);
                }
                else if (input.Equals("2"))
                {
                    await MarketHandler.OrdersHandler(services, p);
                }
                else if (input.Equals("3"))
                {
                    await MarketHandler.PositionHandler(services, p);
                }
                else if (input.Equals("4"))
                {
                    await MarketHandler.SellAll(services, p);
                }
                else if (input.Equals("5"))
                {
                    await SendDiscord(services, p);   
                }
                else if (args.Length == 1)
                {
                    await MarketHandler.QuoteHandler(services, input);
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
                        Console.WriteLine($"\nError: {amount} is not a valid amount.\n");
                        continue;
                    }
                    if (int.Parse(amount) < 0)
                    {
                        Console.WriteLine($"\nError: Amount must be greater than 0.\n");
                        continue;
                    }
                    if (stock == null || stock.Length == 0)
                    {
                        Console.WriteLine($"\nError: {stock ?? "empty" }is not a valid stock!\n");
                        continue;
                    }
                    await MarketHandler.PlaceOrderHandler(services, p, stock, amount, type, true);
                }
            } while (!input.ToUpper().Equals("Q"));
        }

        public static async Task SendDiscord(IServiceProvider services, Profile p, List<OrderSummary>? orders = null)
        {
            StringBuilder sb = new StringBuilder();
            var discord = services.GetRequiredService<IDiscordService>();
        }


        public static Environments? SetEnvironment()
        {
            int NumOfTries = 3;

            Console.WriteLine("Enter (1) for Production or (2) for SandBox:");
            string? selection = Console.ReadLine() ?? "";

            while (((selection == null || selection.Length == 0) || (!selection.Equals("1") && !selection.Equals("2"))) && NumOfTries-- > 0)
            {
                Console.WriteLine("Error: Enter (1) for Production or (2) for SandBox:");
                selection = Console.ReadLine() ?? "";
            }

            if (NumOfTries <= 0)
            {
                Console.WriteLine("Too many wrong attempts, aborting...");
                return null;
            }
            return selection!.Equals("1") ? Environments.PROD : Environments.SANDBOX;

        }

    }
}
