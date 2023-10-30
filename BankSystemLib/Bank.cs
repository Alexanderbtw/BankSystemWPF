using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace BankSystem.Model
{
    public class Bank
    {
        public static ObservableCollection<Client> Clients { get; set; }

        static Bank()
        {
            Clients = new ObservableCollection<Client>();
        }

        public static void YearLater()
        {
            foreach (var client in Clients)
            {
                client.YearLater();
            }
        }

        public static void MonthLater()
        {
            foreach (var client in Clients)
            {
                client.MonthLater();
            }
        }

        public static void AddClient(string name, string status)
        {
            int id = GetClientId();

            switch (status)
            {
                case "VIP":
                    Clients.Add(new VIP(id, name, status));
                    break;
                case "Individual":
                    Clients.Add(new Individual(id, name, status));
                    break;
                case "Entity":
                    Clients.Add(new Entity(id, name, status));
                    break;
                default:
                    break;
            }
        }

        public static void TransAnotherClient(Client clientFrom, Client clientTo, double money)
        {
            clientFrom.Accounts[0].WithdrawMoney(money + (money * clientFrom.Commission));
            clientTo.Accounts[0].DepositMoney(money);
        }

        private static int GetClientId()
        {
            if (Clients.Count != 0)
            {
                int[] nums = Clients.Select(x => x.Id).ToArray();
                int[] missingNums = Enumerable.Range(nums[0], nums[nums.Length - 1]).Except(nums).ToArray();
                return missingNums.Length == 0 ? nums.Max() + 1 : missingNums.FirstOrDefault();
            }
            else
            {
                return 0;
            }
        }

        public static void SaveToJson()
        {
            string jsonContent = JsonConvert.SerializeObject(Clients, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (!File.Exists("data.json"))
                File.Create("data.json").Close();

            File.WriteAllText("data.json", jsonContent);
        }

        public static void LoadFromJson()
        {
            if (!File.Exists("data.json"))
                return;

            string jsonString = File.ReadAllText("data.json");

            Clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
