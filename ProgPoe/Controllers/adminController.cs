using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProgPoe.Models;
using System.Collections.Generic;

namespace ProgPoe.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult ApproveClaims()
        {
            var adminID = HttpContext.Session.GetInt32("adminID");
            if (adminID.HasValue)
            {
                claimOps claimOpsInstance = new claimOps();
                List<dClaim> claims = claimOpsInstance.GetClaimsFromDatabaseAdmin();
                return View("~/Views/Home/approveClaims.cshtml", claims);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        public IActionResult UpdateClaimStatus(int claimID, string comment, string action)
        {
            var claimOps = new claimOps();
            string status = action == "approve" ? "Approved" : "Rejected";
            bool updateSuccess = claimOps.UpdateClaimStatusAndComment(claimID, status, comment);

            if (updateSuccess)
            {
                return RedirectToAction("ApproveClaims");
            }
            else
            {
                // Handle the error case
                return View("Error");
            }
        }
    }
}
