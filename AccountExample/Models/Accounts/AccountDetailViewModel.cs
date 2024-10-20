namespace AccountExample.Models.Accounts
{
    public class AccountDetailViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Number { get; set; }
        public string Iban { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; } 
    }
}
