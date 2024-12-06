using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TournamentProject.Data;
using TournamentProject.Models;
using TournamentProject.ViewModels;

namespace TournamentProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ApplicationDBContext dbContext, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;


        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


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
                    Email = model.Email,
                    ExpiredDate = DateTime.Now.AddMinutes(1),
                };

                var result = await _userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    TempData["message"] = "شما با موفقیت در سایت ثبت نام شده اید";

                    MailMessage mailMessage = new MailMessage("Sob.kh121@gmail.com", user.Email!);
                    mailMessage.Subject = "تایید حساب کاربری";
                    mailMessage.IsBodyHtml = true;
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string address = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new
                        {
                            userId = user.Id,
                            token = token
                        },
                            Request.Scheme)!;


                    mailMessage.Body = $"Hi <b>{user.Name}<b>" +
                      $"Please click this <a href='{address}'>LINK</a> to confirm your account";


                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("Sob.kh121@gmail.com", "ionj kruy xgum ditk");
                    smtpClient.Send(mailMessage);
                    return RedirectToAction("PleaseConfrim", "Account");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email!);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "حساب کاربری وجود ندارد،لطفا ثبت نام کنید");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);

                if (!await _userManager.IsEmailConfirmedAsync(user!))
                {

                    ModelState.AddModelError(string.Empty, "لطفا ایمیل خود را تایید کنید");
                    return View(model);
                }

                if (result.Succeeded)
                {

                    if (await _userManager.IsInRoleAsync(user!, "Admin"))
                    {
                        return RedirectToAction("AdminPanel", "Admin");
                    }
                    else if (await _userManager.IsInRoleAsync(user!, "User"))
                    {

                        return RedirectToAction("ManagerPanel", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "رمز عبور یا نام کاربری اشتباه است");
                    return View(model);
                }

            }

            return View(model);
        }



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
            if (result.Succeeded && user.ExpiredDate >= DateTime.Now)
            {
                await _userManager.AddToRoleAsync(user, "User");
                Confirming con = new Confirming();
                con.ID = userId;
                con.IsConfirmed = false;

                _dbContext.Comfirm.Add(con);

                _dbContext.SaveChanges();
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
                if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError(string.Empty, "ایمیل وارد شده تایید نشده ابتدا تایید کنید");
                    return View(model);
                }
                if (user!.EmailConfirmed)
                {
                    MailMessage mailMessage = new MailMessage("Sob.kh121@gmail.com", user.Email!);
                    mailMessage.Subject = "بازیابی رمز عبور";
                    mailMessage.IsBodyHtml = true;

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var confirmationLink = Url.Action(
                        "GetResetPass",
                        "Account",
                        new { userId = user.Id, token = token },
                        Request.Scheme);
                    mailMessage.Body = $"سلام <b>{user.Name}<b>" +
                     $"لطفا روی لینک کلیک کنید برای بازیابی رمز عبور <a href='{confirmationLink}'>LINK</a>";


                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("Sob.kh121@gmail.com", "ionj kruy xgum ditk");
                    smtpClient.Send(mailMessage);



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
                var user = await _userManager.FindByIdAsync(model.UserId!);
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

        public IActionResult PleaseConfrim()
        {
            return View();
        }
        public async Task AssignAdminRole(IServiceProvider serviceProvider, string email)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (!await userManager.IsInRoleAsync(user, "Admin"))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }


    }
}
