using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentProject.Data;
using TournamentProject.Models;
using TournamentProject.ViewModels;

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

                var team = _dbContext!.Teams.FirstOrDefault();
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

        [HttpPost]
        public IActionResult WeighIn([FromForm] WeighInVM weighInVM)
        {
            if (ModelState.IsValid)
            {
                Player player = new Player();
                player.Name = weighInVM.FirstName;
                player.LastName = weighInVM.LastName;
                if (weighInVM.Gender == "male")
                {
                    player.Gender = "مرد";
                }
                else
                {
                    player.Gender = "زن";
                }

                double weight;
                if (double.TryParse(weighInVM.Weigh, out weight) && weight <= 48)
                {
                    player.Weigh = weight.ToString();
                }
                else if (double.TryParse(weighInVM.Weigh, out weight) && weight <= 50)
                {
                    player.Weigh = weight.ToString();
                }
                else if (double.TryParse(weighInVM.Weigh, out weight) && weight <= 52)
                {
                    player.Weigh = weight.ToString();
                }
                else if (double.TryParse(weighInVM.Weigh, out weight) && weight <= 54)
                {
                    player.Weigh = weight.ToString();
                }
                else
                {
                    player.Weigh = weight.ToString();
                }

                int currentYear = 1403;

                int birthYear = weighInVM.Year;
                int age = currentYear - birthYear;

                if (age <= 12)
                {
                    player.Age = "نونهالان";
                }
                else if (age > 12 && age <= 15)
                {
                    player.Age = "نوجوانان";
                }
                else if (age > 15 && age <= 17)
                {
                    player.Age = "جوانان";
                }
                else
                {
                    player.Age = "بزرگسالان";
                }
                player.ManagerName = weighInVM.Coach;

                _dbContext!.Players.Add(player);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "اطلاعات با موفقیت ذخیره شد";

            }



            return View();
        }

        public IActionResult FetchPlayers(string ageGroup)
        {
            // Replace with your logic to fetch players from the database
            var players = _dbContext!.Players
                .Where(p => p.Age == ageGroup)
                .Select(p => new
                {
                    p.ID,
                    p.Name,
                    p.LastName,
                    p.Weigh,
                    p.Age,
                    p.Gender,
                    p.ManagerName
                })
                .ToList();
            foreach (var player in players)
            {
                Console.WriteLine($"Name: {player.Name}, ManagerName: {player.ManagerName}, Age: {player.Age}");
            }


            return Json(players);
        }


        public ActionResult GetUsers()
        {
            var users = (from user in _dbContext!.Users
                         join confirm in _dbContext.Comfirm on user.Id equals confirm.ID
                         select new
                         {
                             user.Id,
                             user.Name,
                             user.Email,
                             confirm.IsConfirmed,

                         }).ToList();


            return Json(users);
        }

        [HttpPost]
        public ActionResult ToggleConfirmation(string id)
        {
            var confirmRecord = _dbContext!.Comfirm.FirstOrDefault(c => c.ID == id);

            if (confirmRecord != null)
            {
                confirmRecord.IsConfirmed = !confirmRecord.IsConfirmed;
                _dbContext!.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GettingManagers()
        {
            // Fetch the managers from the database
            var managers = (from user in _dbContext!.Users
                            join confirm in _dbContext.Comfirm
                            on user.Id equals confirm.ID
                            where confirm.IsConfirmed == true
                            select user.Name).ToList();

            // Pass the list to the view
            ViewBag.Managers = managers;
            Console.WriteLine(ViewBag.Managers);



            if (managers.Any())
            {
                return Json(managers); // Send data as JSON
            }
            else
            {
                return Json(new List<string> { "هیچ مربی موجود نیست" }); // No data message
            }
        }


        public IActionResult ManagerPanel()
        {
            return View();
        }


        [HttpPost]
        [Route("Admin/UpdatePlayer")]
        public IActionResult UpdatePlayer([FromBody] PlayerDto playerDto)
        {
            try
            {
                // Validate input
                if (playerDto == null || playerDto.ID <= 0)
                {
                    return BadRequest("Invalid player data.");
                }

                // Find the player in the database
                var player = _dbContext!.Players.FirstOrDefault(p => p.ID == playerDto.ID);
                if (player == null)
                {
                    return NotFound("Player not found.");
                }

                // Update player details
                player.Name = playerDto.Name;
                player.LastName = playerDto.LastName;
                player.Weigh = playerDto.Weigh;
                player.Gender = playerDto.Gender;
                player.ManagerName = playerDto.ManagerName;

                // Save changes to the database
                _dbContext.SaveChanges();

                return Ok(new { success = true, message = "Player updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetPlayersByManagerAndAgeGroup(string managerName, string ageGroup)
        {
            try
            {
                // Fetch players based on managerName and ageGroup
                var players = _dbContext!.Players
                    .Where(p => p.ManagerName == managerName && p.Age == ageGroup)
                    .Select(p => new
                    {
                        p.ManagerName,
                        p.Weigh,
                        p.Name,
                        p.LastName
                    })
                    .ToList();

                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching players.");
            }
        }

        [HttpGet]

        public IActionResult GetLoggedInManagerName()
        {
            // Replace this with the actual logic to get the logged-in manager's name
            var managerName = User.Identity!.Name ?? "Unknown Manager";
            var findOne = _dbContext!.Users.FirstOrDefault(p => p.UserName == managerName);
            var name = findOne!.Name;
            return Ok(new { name });
        }


        [HttpDelete]
        public IActionResult DeletePlayer(int id)
        {
            try
            {
                var player = _dbContext!.Players.FirstOrDefault(p => p.ID == id);
                if (player == null)
                {
                    return NotFound("Player not found.");
                }

                _dbContext.Players.Remove(player);
                _dbContext.SaveChanges();

                return Ok("Player deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the player.");
            }
        }


        public IActionResult TournamentBracket()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPlayersForTournament(string ageGroup, string weightClass)
        {
            if (ageGroup == "kids")
            {
                ageGroup = "نونهالان";
            }
            List<string> finals = new List<string>();

            var players = _dbContext!.Players
                .Where(p => p.Age == ageGroup)
                .Select(p => new
                {
                    p.ID,
                    FullName = $"{p.Name} {p.LastName}",
                    p.Age
                })
                .ToList();

            if (weightClass == "weight1")
            {
                foreach (var p in players)
                {
                    if (p.Age == "نونهالان")
                    {
                        finals.Add(p.FullName!);
                    }
                }
            }
            return Json(finals);
        }

    }


}
