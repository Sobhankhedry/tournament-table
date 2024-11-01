using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentProject.Areas.Identity.Data;
using TournamentProject.Models;
using TournamentProject.ViewModels;

namespace TournamentProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDBContext _DBContext;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ApplicationDBContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this._DBContext = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByEmailAsync(loginVM.Email);



                var result = await signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, false);

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


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {

                    UserName = registerVM.Name,
                    Name = registerVM.Name,
                    Email = registerVM.Email,

                };

                var result = await userManager.CreateAsync(user, registerVM.Password!);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return View();
        }

    }
}

