using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TristanWebsite.Models;

namespace TristanWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ActivitiesAPI activitiesAPI = ActivitiesAPI.Instance();

            Athlete? athlete = await activitiesAPI.GetAthlete()!;
            List<Activities>? activites = await activitiesAPI.GetActivities()!;


            if (athlete != null && activites != null)
            {
                ActivityStats? activityStats = await activitiesAPI.GetAthleteStats(athlete)!;
                athlete.ActivityStats = activityStats;

                Utilities.parseData(athlete, activites);

                HomeViewModel homeViewModel = new HomeViewModel();
                homeViewModel.athlete = athlete;
                homeViewModel.activities = activites;
                return View(homeViewModel);
            }
            return RedirectToAction("Error");
        }
        
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
