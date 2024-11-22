using Microsoft.AspNetCore.Mvc;
using TournamentProject.Data;
using TournamentProject.Models;
using TournamentProject.ViewModels;

namespace TournamentProject.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _dbContext;


        public HomeController(ApplicationDBContext dbContext, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var homeVM = new HomeVM
            {


            };
            var contactUs = new ContactUs();
            var Referees1 = _dbContext.Referees.ToList();
            var Medals1 = _dbContext.Medals.ToList();
            var Championships1 = _dbContext.Teams.ToList();
            var Coaches1 = _dbContext.Coaches.ToList();

            var viewModel = new MultipleVM
            {
                HomeVM = homeVM,
                ContactUs = contactUs,
                Referees = Referees1,
                Medal = Medals1,
                Coaches = Coaches1,
                Teams = Championships1
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ContactUs(ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ContactUs.Add(contactUs);
                _dbContext.SaveChanges();

            }
            return RedirectToAction("Index", "Home");
        }

    }
}
