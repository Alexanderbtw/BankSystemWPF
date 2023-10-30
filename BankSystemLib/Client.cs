using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BankSystem.Model;
using Newtonsoft.Json;

namespace BankSystem.Model
{
    [Serializable]
    public abstract class Client : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        public int Id
        {
            get { return this.id; } 
            set 
            {
                this.id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Id)));
            }
        }

        public string Name { get; protected set; }

        private string status;
        public string Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Status)));
            }
        }

        private ObservableCollection<Account> accounts;
        public ObservableCollection<Account> Accounts
        {
            get { return this.accounts; }
            set
            {
                this.accounts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Accounts)));
            }
        }

        public double Commission { get; protected set; }

        public Client(int id, string name, string status) 
        {
            Id = id;
            Name = name;
            Status = status;
            Accounts = new ObservableCollection<Account> { new Account("Main", "Account", Status) };
            UserLog?.Invoke($"Join new client {this.Name} ({this.Id}) with status: {this.Status}");
            Account.Log += Account_Log;
        }

        public static event Action<string> UserLog;

        private void Account_Log(Account acc, string accInfo)
        {
            if (Accounts.Contains(acc)) 
                UserLog?.Invoke($"{this.Name} ({this.Id}): {accInfo}");
        }

        public void AddAccount(string name, string card_status)
        {

            switch (card_status)
            {
                case "Account":
                    Accounts.Add(new Account(name, card_status, Status));
                    break;
                case "Savings Account":
                    Accounts.Add(new SaveAccount(name, card_status, Status));
                    break;
                case "Contribution":
                    Accounts.Add(new Contribution(name, card_status, Status));
                    break;
                default:
                    break;
            }
            UserLog?.Invoke($"{this.Name} ({this.Id}): Opened a new account {name} with status: {card_status}");
        }

        public void RemoveAccount(Account account)
        {
            UserLog?.Invoke($"{this.Name} ({this.Id}): Deleted account {account.Name} with status: {account.Status}");
            Accounts.Remove(account);
        }

        public void YearLater()
        {
            foreach (var account in Accounts)
            {
                if (account is SaveAccount)
                {
                    ((SaveAccount)account).CalcPercYear();
                }
                else if (account is Contribution) 
                { 
                    ((Contribution)account).CalcPercYear();
                    ReturnMoney(account);
                }
            }
        }

        public void MonthLater()
        {
            foreach (var account in Accounts)
            {
                if (account is SaveAccount)
                {
                    ((SaveAccount)account).CalcPercMonth();
                }
                else if (account is Contribution)
                {
                    ((Contribution)account).CalcPercMonth();
                    if (((Contribution)account).CheckMonthCount())
                    {
                        ReturnMoney(account);
                    }
                }
            }
        }

        public IEnumerable<Account> UsabilityAccounts()
        {
            return Accounts.Where(x => !(x.Status == "Contribution" && x.Money != 0));
        }

        private void ReturnMoney(Account account)
        {
            Accounts[0].DepositMoney(account.Money);
            account.WithdrawMoney(account.Money);
        }
    }
}
