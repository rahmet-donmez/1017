using AccountManagment.Core.Models;

namespace AccountExample.Models.Users
{
    public class UserDetailForAdminViewModel
    {
        public User User { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
