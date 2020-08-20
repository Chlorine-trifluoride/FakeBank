using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace BankModel
{
    public class BankTransaction
    {
        [Key]
        [JsonPropertyName("id")]
        public uint ID { get; set; }

        [JsonPropertyName("amount")]
        public uint Amount { get; set; }

        [JsonPropertyName("senderIBAN")]
        public string SenderIBAN { get; set; }

        [JsonPropertyName("recieverIBAN")]
        public string RecieverIBAN { get; set; }

        [JsonPropertyName("senderName")]
        public string SenderName { get; set; }

        [JsonPropertyName("receiverName")]
        public string ReceiverName { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
