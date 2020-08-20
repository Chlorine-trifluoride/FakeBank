using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankModel
{
    public class BankAccount
    {
        [Key]
        [JsonPropertyName("id")]
        public uint ID { get; set; }

        [JsonPropertyName("ownerID")]
        public uint OwnerID { get; set; }

        [JsonPropertyName("isFrozen")]
        public bool IsFrozen { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }

        [JsonPropertyName("bic")]
        public string BIC { get; set; }

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        public string IBAN => BIC + AccountNumber;
    }
}
