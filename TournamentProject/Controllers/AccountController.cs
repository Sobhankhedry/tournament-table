using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentProject.Models;
using TournamentProject.ViewModels;

namespace TournamentProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        public AccountController(SignInManager<AppUser> signinmanager)
        {
            this.signInManager = signinmanager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, loginVM.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "invalid login attempt");
                    return View(loginVM);
                }

            }
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}

