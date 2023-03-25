using FinFacil.API.Entities;
using FinFacil.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinFacil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly FinFacilDbContext _context;
        public AccountController(FinFacilDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var accounts = _context.Accounts
                .Where(account => account.IsActive)
                .Include(account => account.Currency)
                .Include(account => account.Transactions);

            return Ok(accounts);
        }
        [HttpGet("GetByUser/{userId}")]
        public IActionResult GetByUser(Guid userId)
        {
            var accounts = _context.Accounts
                .Where(account => account.IsActive & account.UserId == userId)
                .Include(account => account.Currency)
                .Include(account => account.Transactions);

            return Ok(accounts);
        }
        [HttpGet("GetByAccountId/{accountId}")]
        public IActionResult GetByAccountId(Guid accountId)
        {
            var accounts = _context.Accounts
                .Where(account => account.IsActive & account.AccountId == accountId)
                .Include(account => account.Currency)
                .Include(account => account.Transactions);

            return Ok(accounts);
        }
        [HttpPost()]
        public IActionResult Post(AccountModel account)
        {
            var userExists = _context.Users.Any(user => user.UserId == account.UserId & !user.IsDeleted);
            if (!userExists)
            {
                return NotFound();
            } 
            else
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetByAccountId), new { accountId = account.AccountId }, account);
            }
        }
        [HttpPut("{accountId}")]
        public IActionResult Update(Guid accountId, AccountModel input)
        {
            var accountExists = _context.Accounts.Any(account => account.AccountId == accountId & account.IsActive);
            if (!accountExists)
            {
                return NotFound();
            }
            else
            {
                var account = _context.Accounts.Single(account => account.AccountId == accountId);
                account.Update(input.Name);
                _context.Accounts.Update(account);
                _context.SaveChanges();

                return NoContent();
            }
        }
        [HttpPut("Activate/{accountId}")]
        public IActionResult Activate(Guid accountId)
        {
            var accountExists = _context.Accounts.Any(account => account.AccountId == accountId);
            if (!accountExists)
            {
                return NotFound();
            }

            var account = _context.Accounts.Single(account => account.AccountId == accountId);

            if (account.IsActive)
            {
                account.Inactivate();
            }
            else
            {
                account.Activate();
            }

            _context.Accounts.Update(account);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
