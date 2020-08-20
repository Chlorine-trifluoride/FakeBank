using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace BankClientApp
{
    class Common
    {
        public static void DisplayErrorBox(string errorText)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(errorText, "ERROR", button, icon);
        }

        public static void DisplaySuccessBox(string message)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(message, "SUCCESS", button, icon);
        }
    }
}
