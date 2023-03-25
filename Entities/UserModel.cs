namespace FinFacil.API.Entities
{
    public class UserModel
    {
        public UserModel()
        {
            //Accounts = new List<AccountModel>();
            UserId = Guid.NewGuid();
            IsDeleted = false;
        }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<AccountModel>? Accounts { get; set; }
        public List<TransactionCategoryModel>? TransactionCategories { get; set; }
        public bool IsDeleted { get; set; }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void UpdatePassword(string password)
        {
            Password = password;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
