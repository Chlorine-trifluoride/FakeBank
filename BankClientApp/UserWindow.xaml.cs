using BankModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankClientApp
{
    public partial class UserWindow : Window
    {
        private BankCustomer loggedCustomer = null;

        public UserWindow()
        {
            InitializeComponent();
        }

        public UserWindow(BankCustomer customer) : base()
        {
            loggedCustomer = customer;
        }

        private async Task ReloadUserAccounts()
        {
            BankCustomer customer = CustomerManager.Instance.LoggedInCustomer;

            List<BankAccount> accounts = await AccountManager.Instance.LoadAccountsForUserAsync(customer);

            if (accounts is null || accounts.Count == 0)
                return;

            userAccountsComboBox.ItemsSource = from account in accounts
                                               select $"{account.IBAN}\t{account.BalanceEur}";

            // Keep the same selection, otherwise set to 0
            userAccountsComboBox.SelectedIndex = (userAccountsComboBox.SelectedIndex == -1) ? 0 : userAccountsComboBox.SelectedIndex;
        }

        private async void AccountsDropdownInit(object sender, EventArgs e)
        {
            await ReloadUserAccounts();
        }

        private async void userAccountsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ReloadSelectedAccount();
        }

        private async Task ReloadSelectedAccount()
        {
            int comboIndex = userAccountsComboBox.SelectedIndex;

            // What is calling this with -1? Is that automatic when ItemsSource is changed
            if (comboIndex < 0 || comboIndex > AccountManager.Instance.allUserAccounts.Count)
            {
                //Common.DisplayErrorBox("Account Selection is less than 1\n" +
                //    "Canceling reload");
                return;
            }

            uint accountIndex = AccountManager.Instance.allUserAccounts[comboIndex].ID;

            AccountManager.Instance.SelectedAccount = await AccountManager.Instance.LoadAccountDataAsync(accountIndex);
            UpdateUIAccountSelectionChanged(AccountManager.Instance.SelectedAccount);

            await ReloadTransactionHistoryAsync();
        }

        private void UpdateUIAccountSelectionChanged(BankAccount account)
        {
            // Main window
            balanceLabel.Content = account.BalanceEur;

            // Info Tab
            infoAccountID.Text = account.ID.ToString();
            infoIsActive.Text = (!account.IsFrozen).ToString();
            infoBalance.Text = account.BalanceEur;
            infoBic4.Text = account.BIC;
            infoAccountNumber.Text = account.AccountNumber;
            infoIBAN.Text = account.CombineIBAN;
        }

        private async Task ReloadTransactionHistoryAsync()
        {
            List<BATransaction> transactionHistory = 
                await TransactionManager.BuildTransactionHistoryForAccount(AccountManager.Instance.SelectedAccount);

            // Update the UI
            transactionHistoryListView.ItemsSource = transactionHistory;
        }

        private void WelcomeLabelCreated(object sender, EventArgs e)
        {
            welcomeLabel.Content = $"Welcome {CustomerManager.Instance.LoggedInCustomer.FirstName}";
        }

        private async void CreateNewAccountButtonClick(object sender, RoutedEventArgs e)
        {
            if (Utils.IsAnyEmptyOrNull(createNewAccountBalanceTextBox.Text))
            {
                Common.DisplayErrorBox("Balance field is empty");
                return;
            }

            int balance = 0;
            if (!int.TryParse(createNewAccountBalanceTextBox.Text, out balance))
            {
                Common.DisplayErrorBox("Invalid balance entered");
                return;
            }

            await AccountManager.Instance.CreateNewAccountAsync(balance, CustomerManager.Instance.LoggedInCustomer.ID);
            Common.DisplaySuccessBox("New Bank Account Created");
            await ReloadUserAccounts();
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            CustomerManager.Instance.LoggedInCustomer = null;

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private async void SendMoneyButtonClick(object sender, RoutedEventArgs e)
        {
            string rIBAN = sendRecipientIBAN.Text;
            string rName = sendRecipientName.Text;
            string message = sendMessage.Text;

            uint amount;
            if (!uint.TryParse(sendAmount.Text, out amount))
            {
                Common.DisplayErrorBox("Invalid Syntax for Amount\n" +
                    "Only positive integers allowed.");
                return;
            }

            if (Utils.IsAnyEmptyOrNull<string>(rIBAN, rName))
            {
                Common.DisplayErrorBox("Missing required information");
                return;
            }

            string apiResponse = await TransactionManager.SendTransactionAsync(AccountManager.Instance.SelectedAccount,
                                                                               rIBAN, rName, amount, message);

            await Task.Run(async () =>
                { // this should take about 1 second
                    for (int i = 1; i < 61; i++)
                    {
                        await Dispatcher.BeginInvoke(new Action(delegate() { sendProgressBar.Value = i / 60.0d * 100; }));
                        await Task.Delay(16);
                    }
                });

            // TODO: There's some weird issues here
            //await ReloadSelectedAccount();
            await ReloadUserAccounts();

            // Hack: Update the comboBox item text locally
            int localBalance = (int)(AccountManager.Instance.SelectedAccount.Balance - amount);
            userAccountsComboBox.SelectedItem = $"{AccountManager.Instance.SelectedAccount.IBAN}\t{AccountManager.Instance.SelectedAccount.BalanceEur}"; 

            // TODO: Display more userfriendly information
            Common.DisplaySuccessBox("Money transfered succesfully");
        }

        private void InfoIBANCopyToClipButtonClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(infoIBAN.Text);
            Common.DisplaySuccessBox("Account IBAN copied to clipboard");
        }
    }
}
