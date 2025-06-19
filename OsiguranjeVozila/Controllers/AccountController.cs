using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OsiguranjeVozila.Models.ViewModels;

namespace OsiguranjeVozila.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) //provjerava podatke za logovanje
        {
            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password,
                false,false);

            if(signInResult.Succeeded && signInResult!=null)
            {
                return RedirectToAction("List", "Izvjestaj");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() // logout
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("List", "Izvjestaj");
        }

        [HttpGet]
        public IActionResult AccessDenied() //napomena da samo admin moze posjetit odredjene strane
        {
            return View("AccessDenied");
        }
    }
}
