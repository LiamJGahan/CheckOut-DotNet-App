using CheckOut.Data;
using CheckOut.Helpers;
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

        [HttpGet("LoginFailed")]
        public IActionResult LoginFailed()
        {
            return View();
        }

        [HttpGet("LoginSuccess")]
        public IActionResult LoginSuccess()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = await _context.Users
                .FirstOrDefaultAsync(user => user.Username == username);

            var hashedPassword = PasswordHash.Hash(password);

            if (hashedPassword == null) 
            { 
                return RedirectToAction("LoginFailed", "Users"); 
            }

            if (account == null || account.Password != hashedPassword)
            {
                return RedirectToAction("LoginFailed", "Users");
            }
            else
            {
                if (account.Password == hashedPassword)
                {
                    HttpContext.Session.SetInt32("UserId", account.UserId);
                    HttpContext.Session.SetString("Username", account.Username);

                    return RedirectToAction("LoginSuccess", "Users");
                }
                else
                {
                    return RedirectToAction("LoginFailed", "Users"); 
                }
            }
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogoutSuccess", "Users");
        }

        [HttpGet("LogoutSuccess")]
        public IActionResult LogoutSuccess()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpGet("RegisterFailed")]
        public IActionResult RegisterFailed()
        {
            return View();
        }

        [HttpGet("RegisterSuccess")]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(string username, string password, string confirmation)
        {
            if (password != confirmation){ return RedirectToAction("RegisterFailed", "Users"); }

            List<User> accounts = await _context.Users.ToListAsync();
            User user = new User();
            int highestId = 0;
            
            foreach (User account in accounts)
            {
                if (highestId < account.UserId){ highestId = account.UserId; }
                
                if (account.Username == username)
                {
                    return RedirectToAction("RegisterFailed", "Users");
                }
            }

            var hashedPassword = PasswordHash.Hash(password);
            if (hashedPassword == null) 
            { 
                return RedirectToAction("RegisterFailed", "Users"); 
            }

            user.Username = username;
            user.Password = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("RegisterSuccess", "Users");
        }
    }
}