using FinFacil.API.Entities;
using FinFacil.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinFacil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly FinFacilDbContext _context;
        
        public UsersController(FinFacilDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _context.Users
                .Where(user => !user.IsDeleted)
                .Include(user => user.Accounts);

            return Ok(users);
        }
        [HttpGet("{userId}")]
        public IActionResult GetById(Guid userId)
        {
            var userExists = _context.Users.Any(user => user.UserId == userId & !user.IsDeleted);
            if ( !userExists )
            {
                return NotFound();
            }
            var user = _context.Users.Single(user => user.UserId == userId);
            return Ok(user);
        }
        [HttpPost]
        public IActionResult Post(UserModel user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { userId = user.UserId }, user);
        }

        [HttpPatch("{userId}")]
        public IActionResult Update(Guid userId, UserModel input)
        {
            var userExists = _context.Users.Any(user => user.UserId == userId & !user.IsDeleted);
            
            if ( !userExists )
            {
                return NotFound();
            } 
            else
            {
                var user = _context.Users.Single(user => user.UserId == userId);
                user.Update(input.Name, input.Email);
                _context.Users.Update(user);
                _context.SaveChanges();

                return Ok();
            }
        }

        [HttpPatch("password/{userId}")]
        public IActionResult UpdatePassword(Guid userId, [FromBody] UserModel input)
        {
            var userExists = _context.Users.Any(user => user.UserId == userId & !user.IsDeleted);

            if (!userExists)
            {
                return NotFound();
            }
            else
            {
                var user = _context.Users.Single(user => user.UserId == userId);
                user.UpdatePassword(input.Password);
                _context.Users.Update(user);
                _context.SaveChanges();

                return NoContent();
            }
        }

        [HttpDelete("{userId}")]
        public IActionResult Delete(Guid userId)
        {
            var userExists = _context.Users.Any(user => user.UserId == userId & !user.IsDeleted);

            if (!userExists)
            {
                return NotFound();
            }
            else
            {
                var user = _context.Users.Single(user => user.UserId == userId);
                user.Delete();
                _context.Users.Update(user);
                _context.SaveChanges();

                return NoContent();
            }
        }
    }
}
