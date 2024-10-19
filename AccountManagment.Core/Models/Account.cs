using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Core.Models
{
    public class Account:BaseEntity
    {
        public string Number { get; set; }
        public string Iban { get; set; }
        public decimal Balance { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
