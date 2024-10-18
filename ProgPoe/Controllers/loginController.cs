using Microsoft.AspNetCore.Mvc;
using ProgPoe.Models;

namespace ProgPoe.Controllers
{
    public class loginController : Controller
    {
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var login = new userTable();

            int userID = login.FetchUser(email, password);
            if (userID != -1)
            {
                HttpContext.Session.SetInt32("userID", userID); //Code for the cookie was learned from this website https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-8.0
                return RedirectToAction("Index", "Home", new { userID = userID });

            }
            else
            {
                return View("~/Views/Home/loginPage.cshtml");

            }
        }

        [HttpPost]
        public ActionResult adminLogin(string email, string password)
        {
            var login = new userTable();

            int userID = login.FetchUserAdmin(email, password);
            if (userID != -1)
            {
                HttpContext.Session.SetInt32("adminID", userID); //Code for the cookie was learned from this website https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-8.0
                return RedirectToAction("Index", "Home", new { adminID = userID });

            }
            else
            {
                return View("~/Views/Home/loginPage.cshtml");

            }
        }
    }
}