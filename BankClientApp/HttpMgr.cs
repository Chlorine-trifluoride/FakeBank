using BankModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankClientApp
{
    class HttpMgr
    {
        public static HttpMgr Instance = new HttpMgr();
        private const int PORT = 44314;

        private HttpMgr()
        {
        }

        private HttpClientHandler GetNewHandler()
        {   // No idea why this can't be reused
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            // This will allow unsigned SSL certificates
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            return httpClientHandler;
        }

        public async Task<string> PostAccountAsync(BankAccount account)
        {
            string apiResponse = "";

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                string data = JsonSerializer.Serialize<BankAccount>(account);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"https://localhost:{PORT}/api/BankAccounts", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return apiResponse;
        }

        public async Task<List<BankAccount>> GetAllAccountsAsync()
        {
            List<BankAccount> bankAccounts;

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"https://localhost:{PORT}/api/BankAccounts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(apiResponse);
                }
            }

            return bankAccounts;
        }

        public async Task<BankAccount> GetAccountDataAsync(uint id)
        {
            BankAccount bankAccount;

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"https://localhost:{PORT}/api/BankAccounts/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankAccount = JsonSerializer.Deserialize<BankAccount>(apiResponse);
                }
            }

            return bankAccount;
        }

        public async Task<List<BankAccount>> GetAccountsForUserAsync(uint userId, string passwordHash)
        {
            List<BankAccount> bankAccounts;

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"https://localhost:{PORT}/api/BankAccounts/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(apiResponse);
                }
            }

            return bankAccounts;
        }

        public async Task<string> PostCustomerAsync(BankCustomer customer)
        {
            string apiResponse = "";

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                string data = JsonSerializer.Serialize<BankCustomer>(customer);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"https://localhost:{PORT}/api/BankCustomers", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return apiResponse;
        }

        public async Task<List<BankCustomer>> GetAllCustomersAsync()
        {
            List<BankCustomer> bankCustomers;

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"https://localhost:{PORT}/api/BankCustomers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankCustomers = JsonSerializer.Deserialize<List<BankCustomer>>(apiResponse);
                }
            }

            return bankCustomers;
        }
    }
}
