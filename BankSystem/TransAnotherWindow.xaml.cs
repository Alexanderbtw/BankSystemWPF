using BankSystem.Model;
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

namespace BankSystem
{
    /// <summary>
    /// Логика взаимодействия для TransAnotherWindow.xaml
    /// </summary>
    public partial class TransAnotherWindow : Window
    {
        private Client clientFrom;
        private readonly List<Key> num_keys = new List<Key> { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0 };

        public TransAnotherWindow(Client clnt)
        {
            clientFrom = clnt;
            InitializeComponent();
            cbClientTo.ItemsSource = Bank.Clients.Where(x => x != clientFrom);
            tbOnAccount.Text = "On this Main account: " + clientFrom.Accounts[0].Money;
        }

        private void tbAmountMoney_KeyPressed(object sender, KeyEventArgs e)
        {
            if (num_keys.Contains(e.Key))
            {
                if (tbAmontMoney.Text.Contains(',') && tbAmontMoney.Text.Split(',')[1].Count() >= 2)
                    e.Handled = true;
                else
                    return;
            }

            else if (e.Key == Key.OemComma && !tbAmontMoney.Text.Contains(','))
                return;

            else if (e.Key == Key.Back)
                return;

            e.Handled = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (cbClientTo.SelectedItem == null) { return; }

            try
            {
                double money = double.Parse(tbAmontMoney.Text);

                if (clientFrom.Accounts[0].WithdrawMoney(money + CalcCommission()))
                {
                    Client clientTo = cbClientTo.SelectedItem as Client;
                    clientTo.Accounts[0].DepositMoney(money);
                    Close();
                }
                else
                {
                    MessageBox.Show("Not enough money!");
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("Too much money!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Money don't intered!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Null Reference!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private double CalcCommission()
        {
            double commission = 0;
            if (tbAmontMoney.Text != "")
            {
                commission = Math.Round(double.Parse(tbAmontMoney.Text) * clientFrom.Commission, 2);
            }
            return commission;
        }

        private void tbAmountMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbCommisionWillBe.Text = "Commission will be: " + CalcCommission().ToString();
        }
    }
}
