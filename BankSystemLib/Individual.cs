using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model
{
    internal class Individual : Client
    {
        public Individual(int id, string name, string status)
            : base(id, name, status) 
        {
            Commission = 0.03;
        }
    }
}
