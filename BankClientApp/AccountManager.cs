using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankModel;

namespace BankClientApp
{
    class AccountManager
    {
        public static AccountManager Instance = new AccountManager();
        private static List<BankAccount> allAccounts = null;
        public List<BankAccount> allUserAccounts = null;
        public BankAccount SelectedAccount { get; set; }

        public async Task<string> CreateNewAccountAsync(int balance, uint ownerId, bool isFrozen = false)
        {
            uint newAccountIndex = newAccountIndex = (uint)(allUserAccounts?.Count ?? 0);
            string newAccountNumber = Utils.CreateBankAccountNumber(CustomerManager.Instance.LoggedInCustomer, newAccountIndex);

            BankAccount account = new BankAccount
            {
                OwnerID = ownerId,
                IsFrozen = isFrozen,
                Balance = balance,
                BIC = GLOBALS.BANK_BIC,
                AccountNumber = newAccountNumber
            };
            // TODO: fix this maddnessss
            account.IBAN = account.CombineIBAN;

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

        public async Task<BankAccount> LoadAccountDataAsync(uint id)
        {
            return await HttpMgr.Instance.GetAccountDataAsync(id);
        }

        public async Task<List<BankAccount>> LoadAccountsForUserAsync(BankCustomer customer)
        {
            List<BankAccount> userAccounts = await HttpMgr.Instance.GetAccountsForUserAsync(customer.ID, customer.passwordHash);
            allUserAccounts = userAccounts;

            return userAccounts;
        }
    }
}
