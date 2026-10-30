using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Models;

namespace SmartRecruitment.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }


}
