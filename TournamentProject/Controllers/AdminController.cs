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
        public JsonResult GetPlayersForTournament(string ageGroup, string weightClass, string Gender)
        {
            if (Gender == "male")
            {
                Gender = "مرد";
            }
            else
            {
                Gender = "زن";
            }

            switch (ageGroup)
            {
                case "kids":
                    ageGroup = "نونهالان";
                    break;
                case "teens":
                    ageGroup = "نوجوانان";
                    break;
                case "youth":
                    ageGroup = "جوانان";
                    break;
                case "adults":
                    ageGroup = "بزرگسالان";
                    break;
                default:
                    break;
            }


            var players = _dbContext!.Players
                .Where((p => p.Age == ageGroup && p.Gender == Gender))
                .Select(p => new
                {
                    p.ID,
                    FullName = $"{p.Name} {p.LastName}",
                    p.Age
                })
                .ToList();

            var finals = new List<object>();
            if (weightClass == "weight1")
            {
                finals = players
                    .Where(p => p.Age == ageGroup)
                    .Select(p => new { name = p.FullName })
                    .ToList<object>();
            }
            else if (weightClass == "weight2")
            {
                finals = players
                    .Where(p => p.Age == ageGroup)
                    .Select(p => new { name = p.FullName })
                    .ToList<object>();
            }
            else if (weightClass == "weight3")
            {
                finals = players
                    .Where(p => p.Age == ageGroup)
                    .Select(p => new { name = p.FullName })
                    .ToList<object>();
            }
            else if (weightClass == "weight4")
            {
                finals = players
                    .Where(p => p.Age == ageGroup)
                    .Select(p => new { name = p.FullName })
                    .ToList<object>();
            }
            else if (weightClass == "weight5")
            {
                finals = players
                    .Where(p => p.Age == ageGroup)
                    .Select(p => new { name = p.FullName })
                    .ToList<object>();
            }
            var shuffledList = finals.OrderBy(x => Guid.NewGuid()).ToList();
            return Json(shuffledList);
        }




        public JsonResult GetSavedTournaments()
        {
            var tournaments = _dbContext.Matches // Replace Matches with your actual table if different
                .GroupBy(m => m.TournamentName)
                .Select(g => new
                {
                    TournamentName = g.Key // Ensure this matches your database column
                })
                .ToList();

            return Json(tournaments);
        }


        private static string GetAgeGroupFromTournamentName(string tournamentName)
        {
            // Assuming the format "weightClass-gender-ageGroup"
            var parts = tournamentName.Split('-');
            return parts.Length == 3 ? parts[2] : string.Empty;
        }

        private static string GetGenderFromTournamentName(string tournamentName)
        {
            var parts = tournamentName.Split('-');
            return parts.Length == 3 ? parts[1] : string.Empty;
        }

        private static string GetWeightClassFromTournamentName(string tournamentName)
        {
            var parts = tournamentName.Split('-');
            return parts.Length == 3 ? parts[0] : string.Empty;
        }

        [HttpGet]
        [Route("Admin/CheckTournamentExists")]
        public IActionResult CheckTournamentExists(string tournamentName)
        {
            if (string.IsNullOrWhiteSpace(tournamentName))
            {
                return BadRequest("Tournament name is required.");
            }

            // Check if the tournament already exists
            bool exists = _dbContext!.Matches.Any(m => m.TournamentName == tournamentName);

            return Ok(exists);
        }


        public JsonResult LoadTournament(string tournamentName)
        {
            var matches = _dbContext.Matches
                .Where(m => m.TournamentName == tournamentName)
                .Select(m => new
                {
                    m.BracketNo,
                    m.RoundNo,
                    TeamNames = new[] { m.TeamAName, m.TeamBName },
                    Scores = new[] { m.TeamAScore, m.TeamBScore },
                    m.NextGameId
                })
                .ToList();

            return Json(matches);
        }

        [HttpPost]
        [Route("Admin/SaveBracketData")]
        public IActionResult SaveBracketData([FromBody] SaveBracketRequest request)
        {
            string tournamentName = request.TournamentName;
            List<Bracket> brackets = request.Brackets;


            if (brackets == null || brackets.Count == 0)
            {
                return BadRequest("No data received.");
            }

            var matches = _dbContext!.Matches
            .Where(m => m.TournamentName == tournamentName)
            .ToList();

            if (matches.Count > 0)
            {
                _dbContext.Matches.RemoveRange(matches);
                _dbContext.SaveChanges(); // Commit the changes to the database
            }

            // For each match:
            foreach (var match in brackets)
            {
                var matchEntity = new MatchEntity
                {
                    TournamentName = tournamentName,
                    BracketNo = match.BracketNo,
                    RoundNo = match.RoundNo,
                    TeamAName = match.Teamnames?.Length > 0 ? match.Teamnames[0] : null,
                    TeamBName = match.Teamnames?.Length > 1 ? match.Teamnames[1] : null,
                    TeamAScore = match.Scores?.Length > 0 ? match.Scores[0] : 0,
                    TeamBScore = match.Scores?.Length > 0 ? match.Scores[1] : 0,
                    NextGameId = match.NextGame,
                    // handle LastGames: store them as JSON or another relationship.
                };

                // Insert or update matchEntity in DB
                _dbContext!.Matches.Add(matchEntity); // pseudo code
            }

            // Save changes
            _dbContext!.SaveChanges();

            return Ok(new { message = "Bracket saved successfully" });
        }


        public IActionResult referees()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTournament([FromBody] TournamentNameDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.TournamentName))
            {
                return BadRequest(new { message = "Tournament name is required." });
            }

            try
            {
                var playersToDelete = _dbContext.Matches
                    .Where(p => p.TournamentName.Trim().ToLower() == request.TournamentName.Trim().ToLower())
                    .ToList();

                if (!playersToDelete.Any())
                {
                    return NotFound(new { message = $"No players found for tournament '{request.TournamentName}'." });
                }

                _dbContext.Matches.RemoveRange(playersToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    message = $"Successfully deleted all players for tournament '{request.TournamentName}'.",
                    deletedCount = playersToDelete.Count
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting tournament: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the tournament.", error = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult SaveReferee(Referees referee)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Referee.Add(referee);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Referee saved successfully!" });
            }

            return Json(new { success = false, message = "Validation failed!" });
        }


        [HttpGet]
        public IActionResult GetReferee()
        {
            var referees = _dbContext.Referee.ToList();
            return Json(referees);
        }

        [HttpDelete]
        public IActionResult DeleteReferee(int id)
        {
            var referee = _dbContext.Referee.Find(id);
            if (referee == null)
            {
                return NotFound();
            }

            _dbContext.Referee.Remove(referee);
            _dbContext.SaveChanges();

            return Ok();
        }




        [HttpPost]
        public async Task<IActionResult> Create(AnnouncementViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                // Save the uploaded image
                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }

                    imagePath = "/uploads/" + uniqueFileName;
                }

                // Save the announcement to the database
                var announcement = new Announcement
                {
                    Title = model.Title,
                    Description = model.Description,
                    ImagePath = imagePath,
                    CreatedAt = DateTime.Now
                };

                _dbContext.Announcements.Add(announcement);
                await _dbContext.SaveChangesAsync();

                return Json(new
                {
                    title = announcement.Title,
                    description = announcement.Description,
                    imagePath = announcement.ImagePath
                });
            }

            return BadRequest("Invalid data");
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var announcements = _dbContext.Announcements
                .Select(a => new
                {
                    id = a.Id,
                    title = a.Title,
                    description = a.Description,
                    imagePath = a.ImagePath
                })
                .ToList();

            return Json(announcements);
        }



        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var announcement = await _dbContext.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            // Remove the image file
            if (!string.IsNullOrEmpty(announcement.ImagePath))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", announcement.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _dbContext.Announcements.Remove(announcement);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

    }




}
