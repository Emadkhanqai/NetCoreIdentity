using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace NetCoreIdentity_Ep2.Controllers
{
    public class HomeController : Controller
    {
        // this provided UserManager class provide functionality to CRUD users
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailService emailService;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
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
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

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
        public async Task<IActionResult> Register(string username, string password)
        {
            //create user

            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Generate email token
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                if (code != null)
                {
                    var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
                    await emailService.SendAsync("test@test.com", "Verify Your Email", $"<a href=\"{link}\">Verify Email</a>", true);
                    return RedirectToAction("EmailVerification");
                }
            }

            // Register functionality
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
