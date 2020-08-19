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
        private static List<BankAccount> allAccounts = null;

        public async Task<string> CreateNewAccountAsync(int balance, uint ownerId = 0, bool isFrozen = false)
        {
            BankAccount account = new BankAccount
            {
                OwnerID = ownerId,
                IsFrozen = isFrozen,
                Balance = balance
            };

            return await HttpMgr.Instance.PostAccountAsync(account);
        }

        public async Task<List<BankAccount>> LoadAllAccountsAsync()
        {
            allAccounts = await HttpMgr.Instance.GetAllAccountsAsync();
            return allAccounts;
        }

        public async Task<List<BankAccount>> GetAllAccountsAsync()
        {
            if (allAccounts is null)
                return await LoadAllAccountsAsync();

            return allAccounts;
        }

        public async Task<List<BankAccount>> LoadAccountsForUserAsync(BankCustomer customer)
        {
            return await HttpMgr.Instance.GetAccountsForUserAsync(customer.ID, customer.passwordHash);
        }
    }
}
