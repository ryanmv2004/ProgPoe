using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProgPoe.Models;

namespace ProgPoe.Controllers
{
    public class submitClaimController : Controller
    {
        [HttpPost]
        [Route("submit_claim")]
        public async Task<IActionResult> submitClaim(IFormFile document, string firstname, string lastname, int hoursworked, int hourlyrate, string university)
        {
            if (document == null || document.Length == 0)
            {
                return Content("File not selected");
            }

            // Get the user ID from the session
            int? userID = HttpContext.Session.GetInt32("userID");
            if (userID == null)
            {
                return RedirectToAction("Login", "login");
            }
            String approved = "";

            if (hoursworked < 50 && hourlyrate < 50)
            {
                approved = "Approved";
            }
            else 
            {
                approved = "Pending";
            }

            // Create a new claimOps instance and set properties
            var claim = new claimOps
            {
                LecturerID = userID.Value,
                HoursWorked = hoursworked,
                HourlyRate = hourlyrate,
                ClaimStatus = approved,
                UniName = university
            };

            // Save the claim and document details
            bool isSuccess = await claim.SaveClaimAndDocument(document);

            if (!isSuccess)
            {
                return Content("Error saving claim and document details to database");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

