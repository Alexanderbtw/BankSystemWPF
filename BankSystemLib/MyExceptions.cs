using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemLib
{
    public class LongStringException : Exception
    {
        public string StrMessage { get; set; }

        public LongStringException(string Msg) 
        {
            this.StrMessage = $"This string ({Msg.Substring(0, 5)}...) too much long!";
        }
    }
}
