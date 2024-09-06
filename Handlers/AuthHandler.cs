using Microsoft.Extensions.DependencyInjection;
using RSABot.Abstracts;
using RSABot.Models;


namespace RSABot.Handlers
{
    public class AuthHandler
    {
        public async static Task<bool> ValidateLicense(IServiceProvider services)
        {
            int NumOfTries = 3;

            var validator = services.GetService<IValidateService>() ?? throw new Exception("Error: Missing Validator Service");
            var tokens = services.GetService<ITokenService>() ?? throw new Exception("Error: Missing Token Service");

            if (tokens.GetLicenseKey() == null)
            {
                Console.WriteLine("Please enter your License Key: ");
                string licenseKey = Console.ReadLine() ?? "Null";
                tokens.SaveLicenseKey(licenseKey);
            }

            bool isActive = await validator.ValidateLicense();

            while (!isActive && NumOfTries-- > 0)
            {
                Console.WriteLine("Error: Bad or Inactive License Key. Enter License Key: ");
                var licenseKey = Console.ReadLine() ?? "Null";
                tokens.SaveLicenseKey(licenseKey);
                isActive = await validator.ValidateLicense();
            }

            if (NumOfTries <= 0)
            {
                Console.WriteLine("Too many wrong License Key Attempts, aborting...");
                return false;
            }
            return isActive;

        }

        public async static Task<Profile?> ValidateAPIKey(IServiceProvider services)
        {
            int NumOfTries = 3;

            var tokens = services.GetService<ITokenService>() ?? throw new Exception("Error: Missing Token Service");
            var profileService = services.GetRequiredService<IProfileService>() ?? throw new Exception("Error: Missing Profile Service");

            if (tokens.GetAPIKey() == null)
            {
                Console.WriteLine("Enter your API Token: ");
                var input = Console.ReadLine() ?? "Null";
                tokens.SaveAPIKey(input);
            }

            Profile? p = await profileService.GetProfile();

            while (p == null && NumOfTries-- > 0)
            {
                Console.WriteLine("Error: Bad API Token. Could not authenticate. Try Again: ");
                var input = Console.ReadLine() ?? "Null";
                tokens.SaveAPIKey(input);
                p = await profileService.GetProfile();
            }

            if (NumOfTries <= 0)
            {
                Console.WriteLine("Too many wrong API Key Attempts, aborting...");
                return null;
            }
            return p;
        }

        public async static Task<Profile?> ValidateSandboxKey(IServiceProvider services)
        {
            int NumOfTries = 3;

            var tokens = services.GetService<ITokenService>() ?? throw new Exception("Error: Missing Token Service");
            var profileService = services.GetRequiredService<IProfileService>() ?? throw new Exception("Error: Missing Profile Service");

            if (tokens.GetSandBoxKey() == null)
            {
                Console.WriteLine("Enter your Sandbox API Token: ");
                var input = Console.ReadLine() ?? "Null";
                tokens.SaveSandBoxKey(input);
            }

            Profile? p = await profileService.GetProfile();

            while (p == null && NumOfTries-- > 0)
            {
                Console.WriteLine("Error: Bad Sandbox API Token. Could not authenticate. Try Again: ");
                var input = Console.ReadLine() ?? "Null";
                tokens.SaveSandBoxKey(input);
                p = await profileService.GetProfile();
            }

            if (NumOfTries <= 0)
            {
                Console.WriteLine("Too many wrong API Key Attempts, aborting...");
                return null;
            }
            return p;
        }
    }
}
