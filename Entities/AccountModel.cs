namespace FinFacil.API.Entities
{
    public class AccountModel
    {
        public AccountModel()
        {
            AccountId = Guid.NewGuid();
            Balance = 0;
            IsActive = true;
        }
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public CurrencyModel? Currency { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionModel>? Transactions { get; set; }
        public bool IsActive { get; set; }

        public void Update(string name)
        {
            Name = name;
        }

        public void Inactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
