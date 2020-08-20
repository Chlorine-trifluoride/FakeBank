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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankClientApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DataGrid_Initialized(object sender, EventArgs e)
        {
            WeatherLoader weatherLoader = new WeatherLoader();
            List<WeatherForecast> weatherForecasts = await weatherLoader.LoadWeatherForecasts();
            weatherGrid.ItemsSource = weatherForecasts;
        }

        private async void CreateAccountButtonClick(object sender, RoutedEventArgs e)
        {
            uint customerId = (uint)newAccountOwnerListBox.SelectedIndex;

            string result = await AccountManager.Instance.CreateNewAccountAsync(int.Parse(newAccountBalanceTextbox.Text), customerId);
            debugTextBox.AppendText($"{result}");

            DisplayMessageBox("Account created");
        }

        private void DisplayMessageBox(string message, bool error = false)
        {
            string caption = error ? "ERROR" : "Success";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(message, caption, button, icon);
        }

        private async void AccountsDataGridInit(object sender, EventArgs e)
        {
            accountsDataGrid.ItemsSource = await AccountManager.Instance.LoadAllAccountsAsync();
        }

        private async void RegisterUserButtonClick(object sender, RoutedEventArgs e)
        {
            await CustomerManager.Instance.RegisterNewUser(firstNameTextBox.Text, lastNameTextBox.Text);
            debugTextBox.AppendText("Disabled for re implementation");

            DisplayMessageBox("Customer registered");
        }

        private async void CustomersDataGridInit(object sender, EventArgs e)
        {
            customersDataGrid.ItemsSource = await CustomerManager.Instance.LoadAllCustomersAsync();
        }

        private void customersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // TODO: fix this or something
            DisplayMessageBox("Are you sure?", true);
        }

        private async void NewAccountOwnerListBoxInit(object sender, EventArgs e)
        {
            List<BankCustomer> customers = await CustomerManager.Instance.GetAllAccountsAsync();

            var customerNames = from customer in customers
                    select $"{customer.FirstName} {customer.LastName}";

            newAccountOwnerListBox.ItemsSource = customerNames;
        }
    }
}
