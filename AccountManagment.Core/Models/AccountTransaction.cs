using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Core.Models
{
    public class AccountTransaction:BaseEntity
    {
        public string Description { get; set; }
        public bool Direction { get; set; }//true->gelen false->giden
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
