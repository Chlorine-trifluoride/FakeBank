using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BankModel;

namespace BankClientApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            string firstName = registerFirstNameTextBox.Text;
            string lastName = registerLastNameTextBox.Text;
            string username = registerUserNameTextBox.Text;
            string password = registerPasswordBox.Password;

            if (Utils.IsAnyEmptyOrNull(firstName, lastName, username, password))
                Common.DisplayErrorBox("One or more fields is empty");

            BankCustomer createdCustomer = await CustomerManager.Instance.RegisterNewUser(firstName, lastName, username, password);
            Common.DisplaySuccessBox("Account created succesfully");
            Login(CustomerManager.Instance.LoggedInCustomer);
        }

        private void Login(BankCustomer customer)
        {
            CustomerManager.Instance.LoggedInCustomer = customer;

            UserWindow userWindow = new UserWindow();
            userWindow.Show();
            this.Close();
        }

        private async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = loginUsernameTextBox.Text;
            string password = loginPasswordBox.Password;

            if (Utils.IsAnyEmptyOrNull(username, password))
                Common.DisplayErrorBox("One or more fields is empty");

            BankCustomer customer = await CustomerManager.Instance.GetCustomerWithPasswordAsync(username, password);

            if (customer is null)
                Common.DisplayErrorBox("Invalid Login");

            else
                Login(customer);
        }
    }
}
