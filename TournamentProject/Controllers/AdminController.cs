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
            var list = _dbContext!.ContactUs.ToList();

            return View(list);

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
                if (_dbContext!.Referees.Any())
                {
                    var all = _dbContext.Referees.ToList();
                    _dbContext.Referees.RemoveRange(all);
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

        [HttpPost]
        public JsonResult SaveTeams([FromBody] List<string> teams)
        {
            if (teams == null || !teams.Any())
            {
                return Json(new { success = false, message = "هیچ تیمی دریافت نشد" });
            }
            if (teams.Count < 3)
            {
                return Json(new { success = false, message = "تعداد تیم‌ها کافی نیست. همه را وارد کن" });
            }
            try
            {
                if (_dbContext!.Teams.Any())
                {
                    var all = _dbContext!.Teams.ToList();
                    _dbContext.Teams.RemoveRange(all);
                    _dbContext.SaveChanges();
                }

                var newTeam = new Team { First = teams[0], Second = teams[1], Third = teams[2] };
                _dbContext!.Teams.Add(newTeam);
                _dbContext!.SaveChanges();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = "داده‌ها با موفقیت ذخیره شدند." });
        }

        [HttpGet]
        public IActionResult GetTeams()
        {
            try
            {

                var team = _dbContext.Teams.FirstOrDefault();
                if (team == null)
                {
                    return Json(new { success = true, data = new { First = "", Second = "", Third = "" } });
                }


                return Json(new { success = true, data = new { team.First, team.Second, team.Third } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SaveMedals([FromBody] List<Medals> medalsData)
        {

            if (medalsData == null || !medalsData.Any())
            {
                return BadRequest(new { success = false, message = "No medal data provided." });
            }

            try
            {
                foreach (var medal in medalsData)
                {
                    if (string.IsNullOrEmpty(medal.Name) || string.IsNullOrEmpty(medal.Place) || string.IsNullOrEmpty(medal.Age))
                    {
                        return BadRequest(new { success = false, message = "Invalid medal data. Missing fields" });
                    }
                    if (_dbContext!.Medals.Any())
                    {
                        var all = _dbContext!.Medals.ToList();
                        _dbContext.Medals.RemoveRange(all);
                        _dbContext!.SaveChanges();
                    }

                    _dbContext.Medals.Add(medal);
                }
                _dbContext!.SaveChanges();


                return Ok(new { success = true, message = "Medals saved successfully." });
            }
            catch (System.Exception ex)
            {

                return StatusCode(500, new { success = false, message = $"An error occurred while saving medals: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult GetMedals()
        {
            try
            {
                var medals = _dbContext!.Medals.ToList();
                return Json(new { success = true, data = medals });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult WeighIn()
        {
            return View();
        }



    }
}
