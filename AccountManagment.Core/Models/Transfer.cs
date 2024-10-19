using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Core.Models
{
    public class Transfer: BaseEntity
    {
        public string Description { get; set; }
        public int TargetAccountId { get; set; }//hedef hesap
        public Account TargetAccount { get; set; }
        public int SourceAccountId { get; set; }//kaynak hesap
        public Account SourceAccount { get; set; }
    }
}
