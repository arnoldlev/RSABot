using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Helpers
{
    public class Validators
    {
        public static bool IsInt(string val)
        {
            try
            {
                int.Parse(val);
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}
