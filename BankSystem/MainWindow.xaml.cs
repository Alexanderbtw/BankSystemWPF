using BankSystem.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BankSystem;
using System;
using System.Security.Principal;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BankSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Task load_task = new Task(Bank.LoadFromJson);
        public Task save_task = new Task(Bank.SaveToJson);

        public MainWindow()
        {
            Bank.LoadFromJson();
            InitializeComponent();
            load_task.Start();
            load_task.Wait();
            lvClients.ItemsSource = Bank.Clients;
            Client.UserLog += PrintLogs;
        }

        private void PrintLogs(string logInfo)
        {
            tbLogs.Text += $"\n{logInfo}";
        }

        private void lvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client a = lvClients.SelectedItem as Client;
            if (a != null)
            {
                lvAccounts.ItemsSource = a.Accounts;
                btnDelClient.Visibility = Visibility.Visible;
                btnTransAnother.Visibility = Visibility.Visible;
            }
            else
            {
                lvAccounts.ItemsSource = null;
                btnTransAnother.Visibility = Visibility.Collapsed;
            }
        }

        private void lvAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvAccounts.SelectedItem != null && !IsReplContribution(lvAccounts.SelectedItem))
            {
                btnDelAccount.Visibility = Visibility.Visible;
                btnDeposit.Visibility = Visibility.Visible;
                btnWithdraw.Visibility = Visibility.Visible;
            }
            else
            {
                btnDelAccount.Visibility = Visibility.Collapsed;
                btnDeposit.Visibility = Visibility.Collapsed;
                btnWithdraw.Visibility = Visibility.Collapsed;
            }
        }

        private bool IsReplContribution(object selectedItem)
        {
            if (selectedItem is Contribution && ((Contribution)selectedItem).Money != 0)
                return true;
            return false;
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e) =>
            new ClientWindow().Show();

        private void btnDelClient_Click(object sender, RoutedEventArgs e)
        {
            var clnt = lvClients.SelectedItem as Client;
            PrintLogs($"Deleted client {clnt.Name} ({clnt.Id})");
            Bank.Clients.Remove(clnt);
        }

        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            if (lvClients.SelectedItem != null)
                new AccountWindow(lvClients.SelectedItem as Client).Show();
            else
                MessageBox.Show("Client not selected!");
        }

        private void btnDelAccount_Click(object sender, RoutedEventArgs e)
        {
            var account = lvAccounts.SelectedItem as Account;
            var client = lvClients.SelectedItem as Client;
            if (account.Name == "Main")
                MessageBox.Show("Can't delete this!");
            else
                client.RemoveAccount(account);

        }

        private void btnDeposit_Click(object sender, RoutedEventArgs e) =>
            new DepositWindow(lvAccounts.SelectedItem as Account).Show();

        private void btnWithdraw_Click(object sender, RoutedEventArgs e) =>
            new WithdrawWindow(lvAccounts.SelectedItem as Account).Show();

        private void btnTransBtwAcc_Click(object sender, RoutedEventArgs e)
        {
            if (lvClients.SelectedItem != null)
            {
                Client client = lvClients.SelectedItem as Client;
                if (client.UsabilityAccounts().ToList().Count >= 2)
                {
                    new TransBtwAccWindow(lvClients.SelectedItem as Client).Show();
                }
                else
                {
                    MessageBox.Show("The number of usability account is less than 2!");
                }
            }
            else
            {
                MessageBox.Show("Client not selected!");
            }
        }

        private void btnTransAnother_Click(object sender, RoutedEventArgs e)
        {
            if (Bank.Clients.Count >= 2)
                new TransAnotherWindow(lvClients.SelectedItem as Client).Show();
            else
                MessageBox.Show("No one to transfer!");
        }

        private void btnSkipYear_Click(object sender, RoutedEventArgs e) =>
            Bank.YearLater();

        private void btnSkipMonth_Click(object sender, RoutedEventArgs e) =>
            Bank.MonthLater();

        private void Closed_SaveData(object sender, EventArgs e)
        {
            save_task.Start();
            this.Hide();
            save_task.Wait();
        }
    }
}
