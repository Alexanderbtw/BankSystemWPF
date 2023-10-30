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
    /// Логика взаимодействия для WithdrawWindow.xaml
    /// </summary>
    public partial class WithdrawWindow : Window
    {
        private Account account;
        private readonly List<Key> keys = new List<Key> { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0, Key.Back};

        public WithdrawWindow(Account acc)
        {
            InitializeComponent();
            account = acc;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbAmountMoney_KeyPressed(object sender, KeyEventArgs e)
        {
            if (!keys.Contains(e.Key))
            {
                e.Handled = true;
            }
        }

        private void btnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            double money = 0;
            try
            {
                money = double.Parse(tbAmountMoney.Text);
            }
            catch (OverflowException)
            {
                MessageBox.Show("Too much money!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (money % 100 == 0)
            {
                if (account.WithdrawMoney(money))
                {
                    Close();
                }
            }
            MessageBox.Show("Not multiple of 100 or not enough money!");
        }
    }
}
