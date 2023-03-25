using FinFacil.API.Entities;
using FinFacil.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinFacil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly FinFacilDbContext _context;
        public TransactionController(FinFacilDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult Index()
        {
            var transactions = _context.Transactions
                .Where(transaction => !transaction.IsDeleted);

            return Ok(transactions);
        }
        [HttpGet("GetByAccount/{accountId}")]
        public IActionResult GetByAccount(Guid accountId)
        {
            var transactions = _context.Transactions
                .Where(transaction => transaction.AccountId == accountId)
                .Include(transaction => transaction.TransactionCategory);

            return Ok(transactions);
        }
        [HttpGet("GetById/{transactionId}")]
        public IActionResult GetById(Guid transactionId)
        {
            var transaction = _context.Transactions
                .Include(transaction => transaction.TransactionCategory)
                .SingleOrDefault(transaction => transaction.TransactionId == transactionId & !transaction.IsDeleted);

            return Ok(transaction);
        }
        [HttpPost("{transactionCategoryId}")]
        public IActionResult Post(Guid transactionCategoryId, TransactionModel transaction)
        {
            var transactionCategoryExists = _context.TransactionCategories.Any(transactionCategory => transactionCategory.TransactionCategoryId == transactionCategoryId);
            var accountExists = _context.Accounts.Any(account => account.AccountId == transaction.AccountId & account.IsActive);

            if ( !accountExists | !transactionCategoryExists )
            {
                return NotFound();
            }
            else
            {
                transaction.TransactionCategory = _context.TransactionCategories
                    .Single(transactionCategory => transactionCategory.TransactionCategoryId == transactionCategoryId);
                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { transactionId = transaction.TransactionId }, transaction);
            }
        }
        [HttpPut("{transactionId}/{transactionCategoryId}")]
        public IActionResult UpdateTransactionCategory(Guid transactionId, Guid transactionCategoryId)
        {
            var transactionExists = _context.Transactions.Any(transaction => transaction.TransactionId == transactionId);
            var transactionCategoryExists = _context.TransactionCategories.Any(transactionCategory => transactionCategory.TransactionCategoryId == transactionCategoryId);
            
            if ( !transactionCategoryExists | !transactionExists )
            {
                return NotFound();
            }
            else
            {
                var transactionCategory = _context.TransactionCategories
                    .Single(transactionCategory => transactionCategory.TransactionCategoryId == transactionCategoryId);
                var transaction = _context.Transactions.Single(transaction => transaction.TransactionId == transactionId);

                transaction.Update(transactionCategory);
                _context.Transactions.Update(transaction);
                _context.SaveChanges();

                return NoContent();
            }
        }
        [HttpDelete("{transactionId}")]
        public IActionResult Delete(Guid transactionId)
        {
            var transactionExists = _context.Transactions.Any(transaction => transaction.TransactionId == transactionId & !transaction.IsDeleted);
            
            if ( transactionExists )
            {
                var transaction = _context.Transactions.Single(transaction => transaction.TransactionId == transactionId);
                transaction.Delete();
                _context.Transactions.Update(transaction);
                _context.SaveChanges();

                return Ok();
            } else
            {
                return NotFound();
            }
        }
    }
}
