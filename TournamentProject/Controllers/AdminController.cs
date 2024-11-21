using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentProject.Data;
using TournamentProject.Models;

namespace TournamentProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext? _dbContext;
        private readonly SignInManager<AppUser> _signInManager;
        public AdminController(ApplicationDBContext dBContext, SignInManager<AppUser> signInManager)
        {
            _dbContext = dBContext;
            _signInManager = signInManager;

        }
        public IActionResult AdminPanel()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SaveCoaches([FromBody] List<string> items)
        {
            if (items == null || !items.Any())
            {
                return Json(new { success = false, message = "هیچ داده‌ای دریافت نشد." });
            }

            try
            {
                if (_dbContext!.Coaches.Any())
                {
                    var all = _dbContext.Coaches.ToList();
                    _dbContext.Coaches.RemoveRange(all);
                    _dbContext.SaveChanges();
                }
                foreach (var name in items)
                {
                    var newCoach = new Coach { Name = name };
                    _dbContext!.Coaches.Add(newCoach);
                }

                _dbContext!.SaveChanges();

                return Json(new { success = true, message = "داده‌ها با موفقیت ذخیره شدند." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GetCoaches()
        {
            var coaches = _dbContext!.Coaches.Select(c => c.Name).ToList();
            return Json(coaches);
        }

        [HttpGet]
        public IActionResult GetReferees()
        {
            var referees = _dbContext!.Referees.Select(r => r.Name).ToList();
            return Json(referees);
        }

        [HttpPost]
        public JsonResult SaveReferees([FromBody] List<string> items)
        {
            if (items == null || !items.Any())
            {
                return Json(new { success = false, message = "هیچ داده‌ای دریافت نشد." });
            }

            try
            {
                if (_dbContext!.Coaches.Any())
                {
                    var all = _dbContext.Coaches.ToList();
                    _dbContext.Coaches.RemoveRange(all);
                    _dbContext.SaveChanges();
                }
                foreach (var name in items)
                {
                    var newRef = new Referee { Name = name };
                    _dbContext!.Referees.Add(newRef);

                }

                _dbContext!.SaveChanges();

                return Json(new { success = true, message = "داده‌ها با موفقیت ذخیره شدند." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
