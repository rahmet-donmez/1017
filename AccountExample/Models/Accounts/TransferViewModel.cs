using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AccountExample.Models.Accounts
{
    public class TransferViewModel
    {
        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar sıfırdan büyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kaynak hesap zorunludur.")]
        public int SourceAccountId { get; set; }

        [Required(ErrorMessage = "Hedef hesap IBAN zorunludur.")]
        public string TargetAccountIban { get; set; }

        public List<SelectListItem>? Accounts { get; set; } // Hesap listesi için
    }
}
