using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity_Ep2.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreIdentity_Ep2.Controllers
{
    public class HomeController : Controller
    {
        // this provided UserManager class provide functionality to CRUD users
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            //create user

            var user = new IdentityUser
            {
                UserName = userName,
                Email = "",
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // sign in the user
            }
            else
            {

            }

            // Register functionality
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
