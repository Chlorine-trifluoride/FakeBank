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
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
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
                                               select account.IBAN ?? account.ID.ToString();

            userAccountsComboBox.SelectedIndex = 0;
        }

        private async void AccountsDropdownInit(object sender, EventArgs e)
        {
            await ReloadUserAccounts();
        }

        private async void userAccountsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int comboIndex = userAccountsComboBox.SelectedIndex;
            uint accountIndex = AccountManager.Instance.allUserAccounts[comboIndex].ID;

            List<BankAccount> bankAccounts = new List<BankAccount>();
            bankAccounts.Add(await AccountManager.Instance.LoadAccountDataAsync(accountIndex));
            //accountDataGrid.ItemsSource = bankAccounts;
        }

        private void WelcomeLabelCreated(object sender, EventArgs e)
        {
            welcomeLabel.Content = $"Welcome {CustomerManager.Instance.LoggedInCustomer.FirstName}";
        }

        private async void CreateNewAccountButtonClick(object sender, RoutedEventArgs e)
        {
            if (Utils.IsAnyEmptyOrNull(createNewAccountBalanceTextBox.Text))
                Common.DisplayErrorBox("Balance field is empty");

            int balance = 0;
            if (!int.TryParse(createNewAccountBalanceTextBox.Text, out balance))
                Common.DisplayErrorBox("Invalid balance entered");

            await AccountManager.Instance.CreateNewAccountAsync(balance, CustomerManager.Instance.LoggedInCustomer.ID);
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
    }
}
