using FinFacil.API.Entities;
using FinFacil.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace FinFacil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionCategoryController : Controller
    {
        private readonly FinFacilDbContext _context;
        public TransactionCategoryController(FinFacilDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var transactionCategories = _context.TransactionCategories;
            return Ok(transactionCategories);
        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(Guid id)
        {
            var transactionCategoryExists = _context.TransactionCategories.Any(category => category.TransactionCategoryId == id);

            if (!transactionCategoryExists)
            {
                return NotFound();
            }
            else
            {
                var transactionCategory = _context.TransactionCategories.Single(category => category.TransactionCategoryId == id);
                return Ok(transactionCategory);
            }
        }
        [HttpPost()]
        public IActionResult Post(TransactionCategoryModel input)
        {
            _context.TransactionCategories.Add(input);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = input.TransactionCategoryId }, input);
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, TransactionCategoryModel input)
        {
            var transactionCategoryExists = _context.TransactionCategories.Any(category => category.TransactionCategoryId == id);
            
            if (!transactionCategoryExists)
            {
                return NotFound();
            } 
            else
            {
                var transactionCategory = _context.TransactionCategories.Single(category => category.TransactionCategoryId == id);
                transactionCategory.Update(input.Name);
                _context.TransactionCategories.Update(transactionCategory);
                _context.SaveChanges();

                return NoContent();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var transactionCategoryExists = _context.TransactionCategories.Any(category => category.TransactionCategoryId == id);

            if (!transactionCategoryExists)
            {
                return NotFound();
            }
            else
            {
                var transactionCategory = _context.TransactionCategories.Single(category => category.TransactionCategoryId == id);
                _context.TransactionCategories.Remove(transactionCategory);
                _context.SaveChanges();

                return NoContent();
            }
        }
    }
}
