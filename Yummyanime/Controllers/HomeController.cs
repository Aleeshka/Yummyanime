using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Infrastructure;

namespace Yummyanime.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataManager _dataManager;

        public HomeController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Domain.Entities.Anime> list = await _dataManager.Anime.GetAnimeAsync();

            ViewBag.TopRated = HelperDTO.TransformAnime(
                list.OrderByDescending(x => x.Rating)
                    .ThenBy(x => x.Title)
                    .Take(12));

            ViewBag.Latest = HelperDTO.TransformAnime(
                list.OrderByDescending(x => x.Year)
                    .ThenBy(x => x.Title)
                    .Take(12));

            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}
