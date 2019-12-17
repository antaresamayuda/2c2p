using System;
using System.Collections.Generic;

namespace _2C2P.Models
{
    // Models returned by AccountController actions.

    public class TransactionViewModel
    {
        public string id { get; set; }

        public string payment { get; set; }

        public string Status { get; set; }
    }
}
