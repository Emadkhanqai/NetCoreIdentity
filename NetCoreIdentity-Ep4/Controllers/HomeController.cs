using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace NetCoreIdentity_Ep4.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Emad"),
                new Claim(ClaimTypes.Email,"Emadkhanqai@gmail.com"),
                new Claim(ClaimTypes.DateOfBirth,"11/12/1991"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("Grandma.says","Very Nice"),
            };

            // verify

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Danish"),
                new Claim("DrivingLicense","A+"),
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "License Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
