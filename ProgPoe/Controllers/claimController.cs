using Microsoft.AspNetCore.Mvc;
using ProgPoe.Models;
using System.Collections.Generic;

namespace ProgPoe.Controllers
{
    public class ClaimController : Controller
    {
        [HttpGet]
        public IActionResult DisplayClaims()
        {

            claimOps claimOpsInstance = new claimOps();
            List<dClaim> claims = claimOpsInstance.GetClaimsFromDatabase();
            return View("~/Views/Home/displayClaim.cshtml", claims);
        }
    }
}
