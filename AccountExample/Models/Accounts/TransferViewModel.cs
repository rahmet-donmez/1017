using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AccountExample.Models.Accounts
{
    public class TransferViewModel
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Source Account is required")]
        public int SourceAccountId { get; set; }

        [Required(ErrorMessage = "Target Account is required")]
        public string TargetAccountIban { get; set; }

        public List<SelectListItem> Accounts { get; set; } // Hesap listesi için
    }
}
