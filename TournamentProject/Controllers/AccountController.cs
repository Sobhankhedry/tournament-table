using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TournamentProject.Data;
using TournamentProject.Models;
using TournamentProject.Services;
using TournamentProject.ViewModels;

namespace TournamentProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;


        public AccountController(ApplicationDBContext dbContext, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, EmailService emailService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }




        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token = token },
                        Request.Scheme);


                    var emailBody = $"<p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.</p>";
                    _emailService.SendEmail(user.Email!, "Email Confirmation", emailBody);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email!);
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

                if (!await _userManager.IsEmailConfirmedAsync(user!))
                {
                    // Custom error message
                    ModelState.AddModelError(string.Empty, "لطفا ایمیل خود را تایید کنید");
                    return View(model);
                }

                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "رمز عبور یا نام کاربری اشتباه است");
                    return View(model);
                }

            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }

            return View("ConfirmEmailFailure");
        }


        public IActionResult RessetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RessetPassword(RessetEmailVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email!);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "ایمیل وارد شده موجود نیست");
                    return View(model);
                }
                if (user!.EmailConfirmed)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var confirmationLink = Url.Action(
                        "GetResetPass",
                        "Account",
                        new { userId = user.Id, token = token },
                        Request.Scheme);


                    var emailBody = $"<p>برای بازیابی به لینک کلیک کنید. <a href = '{confirmationLink}' > لینک </ a > </p>";
                    _emailService.SendEmail(user.Email!, "Resset password", emailBody);
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }

            return View();

        }


        public IActionResult GetResetPass(string userId, string token)
        {
            if (userId == null || token == null)
            {

                return RedirectToAction("Error", "Home");
            }


            var model = new GetResetPassVM { UserId = userId, Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetResetPass(GetResetPassVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token!, model.ConfirmPassword!);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("PasswordChanged");
                    }
                }

            }
            return View(model);
        }
        public IActionResult PasswordChanged()
        {
            return View();
        }

    }
}
