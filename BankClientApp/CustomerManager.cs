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
        private CustomerManager()
        {
        }

        public async Task<string> RegisterNewUser(string firstName, string lastName)
        {
            BankCustomer customer = new BankCustomer
            {
                FirstName = firstName,
                LastName = lastName,
                ID = 0
            };

            return await HttpMgr.Instance.PostCustomerAsync(customer);
        }

        public async Task<List<BankCustomer>> LoadAllCustomersAsync()
        {
            return await HttpMgr.Instance.GetAllCustomersAsync();
        }
    }
}
