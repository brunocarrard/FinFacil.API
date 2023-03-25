using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace FinFacil.API.Entities
{
    public class TransactionCategoryModel
    {
        public TransactionCategoryModel ()
        {
            TransactionCategoryId = Guid.NewGuid ();
        }
        public Guid? TransactionCategoryId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public void Update(string name)
        {
            Name = name;
        }
    }
}
