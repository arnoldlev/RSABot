using Microsoft.Extensions.DependencyInjection;
using RSABot.Abstracts;
using RSABot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Handlers
{
    public class UserHandler
    {
        public static async Task ProfileHandler(IServiceProvider services, Profile p)
        {
            StringBuilder stringBuilder = new();
            var profileService = services.GetRequiredService<IProfileService>() ?? throw new Exception("Error: Missing Profile Service");
            Console.WriteLine("---- [ Profile ] ----");
            if (p.id == "-1")
            {
                Console.WriteLine("Error: No Profile Found");
            }
            else
            {
                double totalBal = 0.0;
                foreach (Account acc in p.account)
                {
                    Balance? bal = await profileService.GetBalance(acc.account_number);
                    stringBuilder.Append($"\t* Account Number: {acc.account_number}\n");
                    if (bal != null)
                    {
                        totalBal += bal.total_equity;
                        stringBuilder.Append($"\t* Equity: {bal.total_equity:C}\n");
                    }
                    stringBuilder.Append($"\t* Classification: {acc.classification} \n\n");
                }
                Console.WriteLine($"(*) {p.id} | Name: {p.name} | # of Accounts: {p.account.Count} | Total Equity: {totalBal:C}");
                Console.WriteLine(stringBuilder.ToString());
            }
        }
    }
}
