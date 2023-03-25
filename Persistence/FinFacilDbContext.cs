using FinFacil.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinFacil.API.Persistence
{
    public class FinFacilDbContext : DbContext
    {
        public FinFacilDbContext(DbContextOptions<FinFacilDbContext> options) : base(options)
        { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
        public DbSet<TypeModel> Types { get; set; }
        public DbSet<CurrencyModel> Currencies { get; set; }
        public DbSet<TransactionCategoryModel> TransactionCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>(entity =>
            {
                entity.HasKey(user => user.UserId);

                entity.Property(user => user.Name)
                    .IsRequired(true)
                    .HasMaxLength(25)
                    .HasColumnType("varchar(25)");

                entity.Property(user => user.Email)
                    .IsRequired(true)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                entity.Property(user => user.Password)
                    .IsRequired(true)
                    .HasMaxLength(50);

                entity.HasMany(user => user.Accounts)
                    .WithOne()
                    .HasForeignKey(account => account.UserId);

                entity.HasMany(user => user.TransactionCategories)
                    .WithOne()
                    .HasForeignKey(transactionCategory => transactionCategory.UserId);
            });

            builder.Entity<AccountModel>(entity =>
            {
                entity.HasKey(account => account.AccountId);

                entity.Property(account => account.Name)
                    .IsRequired(true)
                    .HasMaxLength(25)
                    .HasColumnType("nvarchar(25)");

                entity.HasOne(account => account.Currency)
                    .WithMany();

                entity.HasMany(account => account.Transactions)
                    .WithOne()
                    .HasForeignKey(transaction => transaction.AccountId);
            });

            builder.Entity<TransactionModel>(entity =>
            {
                entity.HasKey(transaction => transaction.TransactionId);

                entity.Property(transaction => transaction.Description)
                    .IsRequired(true)
                    .HasMaxLength(25)
                    .HasColumnType("varchar(25)");

                entity.Property(transaction => transaction.Amount)
                    .IsRequired(true);

                entity.Property(transaction => transaction.Date)
                    .IsRequired(true);

                entity.HasOne(transaction => transaction.TransactionCategory)
                    .WithMany();

                entity.HasOne(transaction => transaction.Type)
                    .WithMany();
            });

            builder.Entity<CurrencyModel>(entity =>
            {
                entity.HasKey(currency => currency.CurrencyId);

                entity.Property(currency => currency.Name)
                    .IsRequired(true);
            });

            builder.Entity<TypeModel>(entity =>
            {
                entity.HasKey(type => type.TypeId);

                entity.Property(type => type.Description)
                    .IsRequired(true)
                    .HasMaxLength(7)
                    .HasColumnType("varchar(7)");
            });

            builder.Entity<TransactionCategoryModel>(entity =>
            {
                entity.HasKey(transactionCategory => transactionCategory.TransactionCategoryId);

                entity.Property(transactionCategory => transactionCategory.Name)
                    .IsRequired(true)
                    .HasMaxLength(25)
                    .HasColumnType("varchar(25)");
            });
        }
    }
}
