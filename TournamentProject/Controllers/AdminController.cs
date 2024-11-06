using Microsoft.AspNetCore.Mvc;

namespace TournamentProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
