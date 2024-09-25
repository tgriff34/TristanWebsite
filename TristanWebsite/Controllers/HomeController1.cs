using Microsoft.AspNetCore.Mvc;

namespace TristanWebsite.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
