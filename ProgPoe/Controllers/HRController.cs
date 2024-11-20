using Microsoft.AspNetCore.Mvc;
using ProgPoe.Models;
using System.Security.Claims;

namespace ProgPoe.Controllers
{
    public class HRController : Controller
    {
        [HttpGet]
        public IActionResult displayLecturers()
        {
            var adminID = HttpContext.Session.GetInt32("adminID");
            if (adminID.HasValue)
            {
                userTable userTableInstance = new userTable();
                List<dLecturer> lec = userTableInstance.getLecturerList();
                return View("~/Views/Home/HRview.cshtml", lec);
            }
            else
            {
                return RedirectToAction("accessDenied", "Home");
            }
        }

        [HttpPost]
        public IActionResult UpdateLecturer(dLecturer lecturer)
        {
            userTable userTableInstance = new userTable();
            userTableInstance.UpdateLecturer(lecturer);
            return RedirectToAction("displayLecturers");
        }
    }
}
