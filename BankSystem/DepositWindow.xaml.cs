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
using BankSystem.Model;

namespace BankSystem
{
    /// <summary>
    /// Логика взаимодействия для DepositWindow.xaml
    /// </summary>
    public partial class DepositWindow : Window
    {
        private Account account;
        private double amount = 0;

        public DepositWindow(Account acc)
        {
            InitializeComponent();
            account = acc;
        }

        private void btnMoney_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                amount += Convert.ToDouble(btn.Content.ToString());
            }
            catch (OverflowException)
            {
                MessageBox.Show("Too much money!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                amount = 0;
            }
            tbAmountMoney.Text = amount.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnDeposit_Click(object sender, RoutedEventArgs e)
        {
            account.DepositMoney(amount);
            Close();
        }
    }
}
