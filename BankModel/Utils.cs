using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankModel
{
    public static class Utils
    {
        public static bool IsAnyEmptyOrNull<T>(params T[] ts)
        {
            foreach (var v in ts)
            {
                if (v is null)
                    return true;

                if ((v as string) == string.Empty)
                    return true;
            }

            return false;
        }

        public static string CreateBankAccountNumber(BankCustomer customer, uint index)
        {
            string accountNumInput = customer.FirstName + customer.LastName + customer.ID;
            string simpleHash = CryptoService.ComputeSimpleHash(accountNumInput);

            string first14Chars = simpleHash.Substring(0, 12);
            string randomPart = HashToNumbers(first14Chars);
            string userIdPart = index.ToString().PadLeft(2, '0');

            return randomPart + userIdPart;
        }

        private static string HashToNumbers(string input)
        {
            char[] chars = input.ToArray();
            uint[] output = new uint[chars.Length];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = (uint)chars[i];
            }

            // return the same number of chars
            return string.Concat(output).Substring(0, input.Length);
        }
    }
}
