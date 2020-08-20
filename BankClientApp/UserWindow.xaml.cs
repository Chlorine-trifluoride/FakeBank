using BankModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public UserWindow()
        {
            InitializeComponent();
        }

        private async void AccountsDropdownInit(object sender, EventArgs e)
        {
            BankCustomer customer = new BankCustomer
            {
                ID = 1,
                passwordHash = "asd"
            };

            List<BankAccount> accounts = await AccountManager.Instance.LoadAccountsForUserAsync(customer);
            userAccountsComboBox.ItemsSource = from account in accounts
                                               select account.ID;
        }

        private async void userAccountsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            uint id = (uint)userAccountsComboBox.SelectedItem;
            List<BankAccount> bankAccounts = new List<BankAccount>();
            bankAccounts.Add(await AccountManager.Instance.LoadAccountDataAsync(id));
            accountDataGrid.ItemsSource = bankAccounts;
        }
    }
}
