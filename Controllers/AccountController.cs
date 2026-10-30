using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Data;
using SmartRecruitment.Models;

namespace SmartRecruitment.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;// variable to hold the database context for accessing the database

    public AccountController(ApplicationDbContext context)
    {
        _context = context;// variable contains instance of the database context for accessing the database
    }

    // Register Page

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // Save User

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            // Check if email already exists

            var existingUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);

            if (existingUser != null)
            {
                ViewBag.Error = "This email is already registered.";

                return View(user);
            }



            var hasher = new PasswordHasher<User>();// Hash the password

            user.Password = hasher.HashPassword(user, user.Password);// Save hashed password

            // Save user

            _context.Users.Add(user);

            _context.SaveChanges();

            TempData["Success"] = "Registration Successful";

            return RedirectToAction("Login");
        }

        return View(user);
    }

    // Login Page

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Find user by email

        var user = _context.Users.SingleOrDefault(x => x.Email == email);

        if (user != null)
        {
            // Verify password

            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Success)
            {
                // Store data in session

                HttpContext.Session.SetInt32("UserId", user.Id);

                HttpContext.Session.SetString("UserName", user.Name);

                HttpContext.Session.SetString("Role", user.Role);

                // Redirect based on role

                if (user.Role == "Candidate")
                {
                    return RedirectToAction("Dashboard", "Candidate");
                }

                return RedirectToAction("Dashboard", "Recruiter");
            }
        }

        ViewBag.Error = "Invalid Credentials";

        return View();
    }

    // Logout

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";

        Response.Headers["Pragma"] = "no-cache";

        Response.Headers["Expires"] = "0";

        return RedirectToAction("Login");
    }
}
