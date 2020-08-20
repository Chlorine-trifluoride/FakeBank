using BankModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankClientApp
{
    class HttpMgr
    {
        public static HttpMgr Instance = new HttpMgr();
        private const string SERVER = "https://localhost";
        private const int PORT = 5001;

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
                using (var response = await httpClient.PostAsync($"{SERVER}:{PORT}/api/BankAccounts", content))
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
                using (var response = await httpClient.GetAsync($"{SERVER}:{PORT}/api/BankAccounts"))
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
                using (var response = await httpClient.GetAsync($"{SERVER}:{PORT}/api/BankAccounts/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankAccount = JsonSerializer.Deserialize<BankAccount>(apiResponse);
                }
            }

            return bankAccount;
        }

        public async Task<List<BankAccount>> GetAccountsForUserAsync(uint userID, string passwordHash)
        {
            List<BankAccount> bankAccounts;

            // fix for the base64 encoding
            passwordHash = Uri.EscapeDataString(passwordHash);

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"{SERVER}:{PORT}/api/BankAccounts/{userID}/{passwordHash}"))
                {
                    if (!response.IsSuccessStatusCode)
                        return null;

                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(apiResponse);
                }
            }

            return bankAccounts;
        }

        public async Task<BankCustomer> PostCustomerAsync(BankCustomer customer)
        {
            BankCustomer bankCustomer;

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                string data = JsonSerializer.Serialize<BankCustomer>(customer);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{SERVER}:{PORT}/api/BankCustomers", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankCustomer = JsonSerializer.Deserialize<BankCustomer>(apiResponse);
                }
            }

            return bankCustomer;
        }

        public async Task<List<BankCustomer>> GetAllCustomersAsync()
        {
            List<BankCustomer> bankCustomers;

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"{SERVER}:{PORT}/api/BankCustomers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankCustomers = JsonSerializer.Deserialize<List<BankCustomer>>(apiResponse);
                }
            }

            return bankCustomers;
        }

        public async Task<BankCustomer> LoginCustomerAsync(string username, string passwordHash)
        {
            BankCustomer bankCustomer;

            // fix for the base64 encoding
            passwordHash = Uri.EscapeDataString(passwordHash);

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"{SERVER}:{PORT}/api/BankCustomers/{username}/{passwordHash}"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return null; // invalid login

                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bankCustomer = JsonSerializer.Deserialize<BankCustomer>(apiResponse);
                }
            }

            return bankCustomer;
        }

        public async Task<string> PostTransactionAsync(BankTransaction transaction)
        {
            string apiResponse = "";

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                string data = JsonSerializer.Serialize<BankTransaction>(transaction);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{SERVER}:{PORT}/api/BankTransactions", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return apiResponse;
        }

        public async Task<bool> DoesAccountExistsAsync(string IBAN)
        {
            bool accountExists = false;

            using (HttpClient httpClient = new HttpClient(GetNewHandler()))
            {
                using (var response = await httpClient.GetAsync($"{SERVER}:{PORT}/api/BankAccounts/{IBAN}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        accountExists = true;
                }
            }

            return accountExists;
        }
    }
}
