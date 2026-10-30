using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Data;

namespace SmartRecruitment.Controllers;

public class RecruiterController : Controller
{
    private readonly ApplicationDbContext _context;

    public RecruiterController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }

    
        return View();
    }
}