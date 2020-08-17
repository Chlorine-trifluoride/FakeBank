using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BankModel;

namespace BankClientApp
{
    class AccountManager
    {
        public static AccountManager Instance = new AccountManager();

        public async Task<string> CreateNewAccountAsync(int balance, uint ownerId = 0, bool isFrozen = false)
        {
            BankAccount account = new BankAccount
            {
                ID = 0,
                OwnerID = ownerId,
                IsFrozen = isFrozen,
                Balance = balance
            };

            return await HttpMgr.Instance.PostAccountAsync(account);
        }

        public async Task<List<BankAccount>> LoadAllAccountsAsync()
        {
            return await HttpMgr.Instance.GetAllAccountsAsync();
        }
    }
}
