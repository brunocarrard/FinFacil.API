using Microsoft.EntityFrameworkCore;
using System;

namespace FinFacil.API.Entities
{
    public class TransactionModel
    {
        public TransactionModel()
        {
            TransactionId = Guid.NewGuid();
            Date = DateTime.Now;
            IsDeleted = false;
        }
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public TransactionCategoryModel? TransactionCategory { get; set; }
        public TypeModel? Type { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
        }
        public void Update(TransactionCategoryModel input)
        {
            TransactionCategory = input;
        }
    }
}
