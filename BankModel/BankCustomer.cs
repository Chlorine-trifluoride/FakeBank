using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankModel
{
    public class BankCustomer
    {
        [Key]
        [JsonPropertyName("iD")]
        public uint ID { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("username")]
        public string Usernname { get; set; }

        [JsonPropertyName("passwordHash")]
        public string passwordHash { get; set; }
    }
}
