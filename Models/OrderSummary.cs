﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSABot.Models
{
    public class OrderSummary
    {
        public required string AccountNumber { get; set; }
        public string? Symbol { get; set; }
        public string? Amount { get; set; }
        public string? Status { get; set; }
    }
}
