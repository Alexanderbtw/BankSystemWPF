using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankSystem.Model
{
    interface CalcPerc
    {
        void CalcPercYear();
        void CalcPercMonth();
    }

    /// <summary>
    /// Account
    /// </summary>
    public class Account : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static event Action<Account, string> Log;

        public string Name { get; set; }

        private double money;
        public double Money
        {
            get { return this.money; }
            set
            {
                this.money = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Money)));
            }
        }

        public string Status { get; protected set; }

        [JsonProperty]
        public string ClientStatus { get; protected set; }

        [JsonConstructor]
        public Account(string name, double money, string status, string client_status)
        {
            Name = name;
            Money = money;
            Status = status;
            ClientStatus = client_status;
        }

        public Account(string name, string status, string client_status)
        {
            Name = name;
            Money = 0;
            Status = status;
            ClientStatus = client_status;
        }

        public void DepositMoney(double amount)
        {
            Money += amount;
            Log?.Invoke(this, $"{amount} money credited to account {this.Name}");
        }

        public bool WithdrawMoney(double amount)
        {
            if (amount > Money)
            {
                return false;
            }
            else
            {
                Money -= amount;
                Log?.Invoke(this, $"{amount} money debited from account {this.Name}");
                return true;
            }
        }
    }

    /// <summary>
    /// Saving Account
    /// </summary>
    public class SaveAccount : Account, CalcPerc
    {
        [JsonConstructor]
        public SaveAccount(string name, double money, string status, string client_status)
            : base(name, money, status, client_status)
        {

        }

        public SaveAccount(string name, string status, string client_status) 
            : base(name, status, client_status)
        {
            
        }

        public void CalcPercYear()
        {
            switch (ClientStatus)
            {
                case "VIP":
                    Money += Math.Round(Money * 0.10, 2);
                    break;
                case "Entity":
                    Money += Math.Round(Money * 0.03, 2);
                    break;
                case "Individual":
                    Money += Math.Round(Money * 0.05, 2);
                    break;
                default:
                    break;
            }
        }

        public void CalcPercMonth()
        {
            switch (ClientStatus)
            {
                case "VIP":
                    Money += Math.Round(Money * (0.10 / 12), 2);
                    break;
                case "Entity":
                    Money += Math.Round(Money * (0.03 / 12), 2);
                    break;
                case "Individual":
                    Money += Math.Round(Money * (0.05 / 12), 2);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Contribution
    /// </summary>
    public class Contribution : Account, CalcPerc
    {
        [JsonProperty]
        private int MonthCount { get; set; }

        [JsonConstructor]
        public Contribution(string name, double money, string status, string client_status, int monthCount)
            : base(name, money, status, client_status)
        {
            MonthCount = monthCount;
        }

        public Contribution(string name, string status, string client_status)
            : base(name, status, client_status)
        {
            MonthCount = 0;
        }

        public void CalcPercYear()
        {
            switch (ClientStatus)
            {
                case "VIP":
                    Money += Math.Round(Money * 0.20, 2);
                    break;
                case "Entity":
                    Money += Math.Round(Money * 0.06, 2);
                    break;
                case "Individual":
                    Money += Math.Round(Money * 0.10, 2);
                    break;
                default:
                    break;
            }
            MonthCount = 0;
        }

        public void CalcPercMonth()
        {
            switch (ClientStatus)
            {
                case "VIP":
                    Money += Math.Round(Money * (0.20 / 12), 2);
                    break;
                case "Entity":
                    Money += Math.Round(Money * (0.06 / 12), 2);
                    break;
                case "Individual":
                    Money += Math.Round(Money * (0.10 / 12), 2);
                    break;
                default:
                    break;
            }
            MonthCount += 1;
        }

        public bool CheckMonthCount()
        {
            if (MonthCount == 12)
            {
                MonthCount = 0;
                return true;
            }
            return false;
        }
    }
}
