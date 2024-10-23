using System.ComponentModel.DataAnnotations;
using AccountManagment.Core.Models;

namespace AccountExample.Models.Users
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
    }
}
