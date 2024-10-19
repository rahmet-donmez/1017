using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Core.Models
{
    public class User: BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TcNo { get; set; }
        public bool IsAdmin { get; set; } = false;
        public ICollection<Account> Accounts { get; set; }
    }
}
