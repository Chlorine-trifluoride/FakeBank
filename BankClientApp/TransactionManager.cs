using BankModel;
using System;
using System.Collections.Generic;
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
        public static async Task<string> SendTransactionAsync(BankAccount senderAccount, string recipientIBAN, uint amount, string message)
        {
            if (senderAccount.Balance < amount)
                return "Insufficient funds";

            if (!await HttpMgr.Instance.DoesAccountExistsAsync(recipientIBAN))
                return "Recipient account does not exists";



            return "";
        }
    }
}
