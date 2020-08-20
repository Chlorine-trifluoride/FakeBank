using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BankModel
{
    public static class CryptoService
    {
        private const string SALT = "dGhpcyBpcyBhIGEgdmVyeSBpbnRlcmVzdGluZyBzYWx0";
        public static string ComputeHash(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input + SALT);
            byte[] hash;

            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(bytes);
            }

            return Convert.ToBase64String(hash);
        }

        public static string ComputeSimpleHash(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] hash;

            using (SHA1 shaM = new SHA1Managed())
            {
                hash = shaM.ComputeHash(bytes);
            }

            return Convert.ToBase64String(hash);
        }
    }
}
