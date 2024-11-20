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
            userTable userTableInstance = new userTable();
            List<dLecturer> lec = userTableInstance.getLecturerList();
            return View("~/Views/Home/HRview.cshtml", lec);
        }
    }
}
