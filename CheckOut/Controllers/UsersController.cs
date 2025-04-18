using CheckOut.Data;
using CheckOut.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckOut.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        private readonly CheckOutContext _context;

        public UsersController(CheckOutContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = await _context.Users
                .FirstOrDefaultAsync(user => user.Username == username);

            if (account == null || account.Password != password)
            {
                // Add a login unsuccessfull page
                return RedirectToAction("Login", "Users");
            }

            HttpContext.Session.SetInt32("UserId", account.UserId);
            HttpContext.Session.SetString("Username", account.Username);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(string username, string password, string confirmation)
        {
            if (password != confirmation){ return RedirectToAction("Register", "Users"); }

            List<User> accounts = await _context.Users.ToListAsync();
            User user = new User();
            int highestId = 0;
            
            foreach (User account in accounts)
            {
                if (highestId < account.UserId){ highestId = account.UserId; }
                
                if (account.Username == username)
                {
                    return RedirectToAction("Register", "Users");
                }
            }

            user.UserId = highestId++;
            user.Username = username;
            user.Password = password;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Users");
        }

        [HttpGet("Test")]
        public IActionResult SessionTest()
        {
            var user = HttpContext.Session.GetString("Username");
            return Content($"Logged in as: {user}");
        }
    }
}