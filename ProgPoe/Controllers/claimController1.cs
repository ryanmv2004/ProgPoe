using Microsoft.AspNetCore.Mvc;
using ProgPoe.Models;

namespace ProgPoe.Controllers
{
    public class claimController1 : Controller
    {
        [HttpGet]
        [Route("display_claims")]
        public IActionResult DisplayClaims()
        {
            claimOps claimOpsInstance = new claimOps();
            List<dClaim> claims = claimOpsInstance.GetClaimsFromDatabase();
            return View("~/Views/Home/displayClaim.cshtml", claims);
        }
    }
}
