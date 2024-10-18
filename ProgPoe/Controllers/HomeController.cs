using Microsoft.AspNetCore.Mvc;
using ProgPoe.Models;
using System.Diagnostics;

namespace ProgPoe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult signUp()
        {
            return View();
        }

        public IActionResult adminLogin()
        {
            return View();
        }

        public IActionResult displayClaim()
        {
            return RedirectToAction("DisplayClaims", "claimController1");
        }

        public IActionResult approveClaims()
        {
            return RedirectToAction("ApproveClaims", "Admin");
        }

        public IActionResult submitClaim()
        {
            return View();
        }

        public IActionResult accessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
