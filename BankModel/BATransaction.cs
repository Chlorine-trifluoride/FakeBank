using System;
using System.Collections.Generic;
using System.Text;

namespace BankModel
{
    // Used for displaying transactions in the UI
    // Do not use for actual transactions
    public class BATransaction
    {
        public bool Sending { get; set; }
        public int Amount { get; set; }
        public string OtherName { get; set; }
        public string OtherIBAN { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public string AmountEur => $"{Amount}€";
    }
}
