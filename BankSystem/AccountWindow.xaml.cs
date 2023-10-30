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
using BankSystemLib;

namespace BankSystem
{
    /// <summary>
    /// Логика взаимодействия для AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private Client client;

        public AccountWindow(Client aclient)
        {
            InitializeComponent();
            client = aclient;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            string name = tbNameAccount.Text;

            try
            {
                if (name.Length > 30) throw new LongStringException(name);
            }
            catch (LongStringException f)
            {
                MessageBox.Show(f.StrMessage, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            
            if (name == "Main" || name.Trim() == "")
            {
                MessageBox.Show("Can't use this name!");
                return;
            }
            TextBlock selectedItem = (TextBlock)cbStatusAccount.SelectedItem;
            if (selectedItem != null)
            {
                Bank.Clients[Bank.Clients.IndexOf(client)].AddAccount(name, selectedItem.Text);
                Close();
            }
            else
            {
                MessageBox.Show("Cathegory don't selected!");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
