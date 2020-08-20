using BankModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankClientApp
{
    class TransactionManager
    {
        public static TransactionManager Instance = new TransactionManager();
        private TransactionManager() { }

        /* <TextBox Name="sendRecipientIBAN" Grid.Row="0" Grid.Column="1" Text="" Width="250" FontSize="13" HorizontalAlignment="Left" Margin="10,0,0,0" VerticalAlignment="Center"/>
                        <TextBox Name="sendRecipientName" Grid.Row="1" Grid.Column="1" Text="" Width="250" FontSize="13" HorizontalAlignment="Left" Margin="10,0,0,0" VerticalAlignment="Center"/>
                        <TextBox Name="sendAmount" Grid.Row="2" Grid.Column="1" Text="" Width="250" FontSize="13" HorizontalAlignment="Left" Margin="10,0,0,0" VerticalAlignment="Center"/>
                        <TextBox Name="sendMessage"*/
        public static async Task<string> SendTransactionAsync(BankAccount senderAccount, string recipientIBAN, string recipientName, uint amount, string message)
        {
            if (senderAccount.Balance < amount)
                return "Insufficient funds";

            if (!await HttpMgr.Instance.DoesAccountExistsAsync(recipientIBAN))
                return "Recipient account does not exists";

            // Build the transaction
            BankTransaction transaction = new BankTransaction
            {
                Amount = amount,
                Message = message,
                ReceiverName = recipientName,
                RecieverIBAN = recipientIBAN,
                SenderIBAN = senderAccount.CombineIBAN,
                SenderName = CustomerManager.Instance.LoggedInCustomer.FullName,
                Date = DateTime.Now
            };

            // Send it to the API
            return await HttpMgr.Instance.PostTransactionAsync(transaction);
        }

        // Load the raw transaction history
        public static async Task<List<BankTransaction>> LoadBankTransactionHistoryAsync(string IBAN)
        {
            return await HttpMgr.Instance.GetBankTransactionHistoryAsync(IBAN);
        }

        public static async Task<List<BATransaction>> BuildTransactionHistoryForAccount(BankAccount account)
        {
            List<BankTransaction> rawTransactions = await LoadBankTransactionHistoryAsync(account.IBAN);
            
            if (rawTransactions is null) // no transactions
                return null;

            BATransaction[] baTransactionData = new BATransaction[rawTransactions.Count];

            for (int i = 0; i < rawTransactions.Count; i++)
            {
                BankTransaction b = rawTransactions[i];
                BATransaction btr;

                // IF we are the sender
                if (b.SenderIBAN == account.IBAN)
                {
                    btr = new BATransaction
                    {
                        Amount = -(int)b.Amount,
                        OtherIBAN = b.RecieverIBAN,
                        OtherName = b.ReceiverName,
                        Sending = true
                    };
                }

                else // We are the reciever
                {
                    btr = new BATransaction
                    {
                        Amount = (int)b.Amount,
                        OtherIBAN = b.SenderIBAN,
                        OtherName = b.SenderName,
                        Sending = false
                    };
                }

                btr.Date = b.Date;
                btr.Message = b.Message;

                baTransactionData[i] = btr;
            }

            return baTransactionData.ToList();
        }
    }
}
