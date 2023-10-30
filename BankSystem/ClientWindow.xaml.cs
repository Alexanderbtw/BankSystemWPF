using BankSystemLib;
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
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            string name = tbNameClient.Text;

            try
            {
                if (name.Length > 30) throw new LongStringException(name);
            }
            catch (LongStringException f)
            {
                MessageBox.Show(f.StrMessage, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            TextBlock selectedItem = (TextBlock)cbStatusClient.SelectedItem;
            if (selectedItem != null)
            {
                if (name.Trim() == "") name = "Unknown";
                Bank.AddClient(name, selectedItem.Text);
                Close();
            }
            else
            {
                MessageBox.Show("Status don't selected!");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
