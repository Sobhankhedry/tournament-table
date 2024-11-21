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
            // Check if medalsData is null or empty
            if (medalsData == null || !medalsData.Any())
            {
                return BadRequest(new { success = false, message = "No medal data provided." });
            }

            try
            {
                // Process each medal and add it to the database
                foreach (var medal in medalsData)
                {
                    // Example: Validate medal data before saving
                    if (string.IsNullOrEmpty(medal.Name) || string.IsNullOrEmpty(medal.Place) || string.IsNullOrEmpty(medal.Age))
                    {
                        return BadRequest(new { success = false, message = $"Invalid medal data. Missing fields: {string.Join(", ", GetMissingFields(medal))}" });
                    }

                    // Add the medal to the database
                    _dbContext.Medals.Add(medal); // Add your medal object to the DbContext
                }

                // Save all medals to the database
                _dbContext.SaveChanges();

                // Return success response
                return Ok(new { success = true, message = "Medals saved successfully." });
            }
            catch (System.Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                return StatusCode(500, new { success = false, message = $"An error occurred while saving medals: {ex.Message}" });
            }
        }

        // Helper method to get missing fields from the medal
        private IEnumerable<string> GetMissingFields(Medals medal)
        {
            var missingFields = new List<string>();

            if (string.IsNullOrEmpty(medal.Name)) missingFields.Add("Name");
            if (string.IsNullOrEmpty(medal.Place)) missingFields.Add("Place");
            if (string.IsNullOrEmpty(medal.Age)) missingFields.Add("Age");

            return missingFields;
        }


    }
}
