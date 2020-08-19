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
                DisplayErrorBox("One or more fields are empty");

            string httpReply = await CustomerManager.Instance.RegisterNewUser(firstName, lastName, username, password);
        }

        private void DisplayErrorBox(string errorText)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(errorText, "ERROR", button, icon);
        }
    }
}
