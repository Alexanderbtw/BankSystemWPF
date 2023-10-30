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
    /// Логика взаимодействия для TransBtwAccWindow.xaml
    /// </summary>
    public partial class TransBtwAccWindow : Window
    {
        private Client client;
        private readonly List<Key> num_keys = new List<Key> { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0 };

        public TransBtwAccWindow(Client clnt)
        {
            InitializeComponent();
            client = clnt;
            cbTransferFrom.ItemsSource = client.UsabilityAccounts();
            cbTransferTo.ItemsSource = client.UsabilityAccounts();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbTransferTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbTransferFrom.ItemsSource = client.UsabilityAccounts().Where(x => x != cbTransferTo.SelectedItem);
        }

        private void cbTransferFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbTransferTo.ItemsSource = client.UsabilityAccounts().Where(x => x != cbTransferFrom.SelectedItem);
        }

        private void tbAmountMoney_KeyPressed(object sender, KeyEventArgs e)
        {
            if (num_keys.Contains(e.Key))
            {
                if (tbAmountMoney.Text.Contains(',') && tbAmountMoney.Text.Split(',')[1].Count() >= 2)
                    e.Handled = true;
                else
                    return;
            }

            else if (e.Key == Key.OemComma && !tbAmountMoney.Text.Contains(','))
                return;

            else if (e.Key == Key.Back)
                return;

            e.Handled = true;
        }

        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (cbTransferFrom.SelectedItem == null || cbTransferTo.SelectedItem == null) { return; }

            try
            {
                double money = double.Parse(tbAmountMoney.Text);

                Account accFrom = cbTransferFrom.SelectedItem as Account;
                Account accTo = cbTransferTo.SelectedItem as Account;

                if (accFrom.WithdrawMoney(money))
                {
                    accTo.DepositMoney(money);
                    Close();
                }
                else
                {
                    MessageBox.Show("Insufficient funds!");
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("Too much money!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (NullReferenceException) 
            {
                MessageBox.Show("Null Reference!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Money don't intered!", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
