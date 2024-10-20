using AccountManagment.Core.Models;

namespace AccountExample.Models.Accounts
{
    public class AccountDetailWithTransacitonViewModel
    {
        public List<AccountTransaction> AccountTransacitons { get; set; }
        public Account AccountDetail { get; set; }
    }
}
