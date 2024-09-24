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

            Athlete athlete = await activitiesAPI.GetAthlete();
            List<Activities> activites = await activitiesAPI.GetActivities();
            ActivityStats activityStats = await activitiesAPI.GetAthleteStats(athlete);

            athlete.ActivityStats = activityStats;

            Debug.WriteLine("================ Athlete ==================");
            Debug.WriteLine(athlete.Id);
            Debug.WriteLine(athlete.FirstName);
            Debug.WriteLine(athlete.LastName);
            Debug.WriteLine(athlete.LastName);
            Debug.WriteLine(athlete.City);
            Debug.WriteLine(athlete.State);
            Debug.WriteLine(athlete.Follower_Count);
            Debug.WriteLine(athlete.ActivityStats.Biggest_Ride_Distance);
            Debug.WriteLine(athlete.ActivityStats.Biggest_Climb_Elevation_Gain);
            Debug.WriteLine(athlete.ActivityStats.Recent_Ride_Totals.Count);
            Debug.WriteLine(athlete.ActivityStats.Recent_Ride_Totals.Distance);
            Debug.WriteLine(athlete.ActivityStats.Recent_Ride_Totals.Elapsed_Time);
            Debug.WriteLine(athlete.ActivityStats.Recent_Ride_Totals.Moving_Time);
            Debug.WriteLine("===========================================");


            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.athlete = athlete;
            homeViewModel.activities = activites;

            return View(homeViewModel);
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
