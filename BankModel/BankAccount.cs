using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankModel
{
    public class BankAccount
    {
        [JsonPropertyName("id")]
        public uint ID { get; set; }

        [JsonPropertyName("ownerID")]
        public uint OwnerID { get; set; }

        [JsonPropertyName("isFrozen")]
        public bool IsFrozen { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }
    }
}
