namespace AccountExample.Models.Accounts
{
    public class AccountTransacitonsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; } = 10m;
        public bool Direction { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
