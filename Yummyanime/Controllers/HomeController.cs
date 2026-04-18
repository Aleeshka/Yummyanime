using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;

namespace Yummyanime.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}
