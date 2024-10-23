using AccountManagment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Core.Models
{
    public class Account : BaseEntity
    {
        public string Number { get; set; }
        public string Iban { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public User User { get; set; }

        public Account()
        {
            Number = GenerateAccountNumber();
            Iban = GenerateIban();
        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return random.Next(1000000, 9999999).ToString();
        }

        private string GenerateIban()
        {
             string countryCode = "TR";
            Random random = new Random();

            string accountNumber = random.Next(100000000, 999999999).ToString();


            return countryCode  + accountNumber+accountNumber+42;
        }

        
    }
}
