using CheckOut.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckOut.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CheckOutContext _context;

        public UsersController(CheckOutContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            List<User> accounts = await _context.Users.ToListAsync();
            
            foreach (User account in accounts)
            {
                if (account.UserId == user.UserId)
                {
                    return BadRequest("User already exists.");
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.UserId }, user);
        }
    }
}