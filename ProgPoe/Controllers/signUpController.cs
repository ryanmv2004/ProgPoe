using Microsoft.AspNetCore.Mvc;
using ProgPoe.Models;

namespace ProgPoe.Controllers
{
    public class signUpController : Controller
    {
        public userTable tbl = new userTable();
        
        [HttpPost]
        public ActionResult SignUp(userTable Users)
        {
            var result = tbl.insert_User(Users);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult adminSignUp(userTable Users)
        {
            var result = tbl.insert_UserAdmin(Users);
            return RedirectToAction("Index", "Home");
        }
    }
}
