using BankModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankClientApp
{
    class CustomerManager
    {
        public static CustomerManager Instance = new CustomerManager();
        public BankCustomer LoggedInCustomer { get; set; } = null;
        private static List<BankCustomer> allCustomers = null;

        private CustomerManager()
        {
        }

        public async Task<BankCustomer> RegisterNewUser(string firstName, string lastName, string username, string password)
        {
            BankCustomer customer = new BankCustomer
            {
                FirstName = firstName,
                LastName = lastName,
                Usernname = username,
                passwordHash = CryptoService.ComputeHash(password)
            };

            // the server replies with added fields
            LoggedInCustomer = await HttpMgr.Instance.PostCustomerAsync(customer);
            return LoggedInCustomer;
        }

        public async Task<BankCustomer> RegisterNewUser(string firstName, string lastName)
        {
            BankCustomer customer = new BankCustomer
            {
                FirstName = firstName,
                LastName = lastName
            };

            return await HttpMgr.Instance.PostCustomerAsync(customer);
        }

        public async Task<List<BankCustomer>> LoadAllCustomersAsync()
        {
            allCustomers = await HttpMgr.Instance.GetAllCustomersAsync();
            return allCustomers;
        }

        public async Task<List<BankCustomer>> GetAllAccountsAsync()
        {
            if (allCustomers is null)
                return await LoadAllCustomersAsync();

            return allCustomers;
        }

        public async Task<BankCustomer> GetCustomerWithPasswordAsync(string username, string password)
        {
            string passwordHash = CryptoService.ComputeHash(password);
            return await HttpMgr.Instance.LoginCustomerAsync(username, passwordHash);
        }
    }
}
