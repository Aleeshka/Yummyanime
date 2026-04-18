using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Domain.Entities;
using Yummyanime.Infrastructure;
using Yummyanime.Models;

namespace Yummyanime.Controllers
{
    public class HomeController : Controller
    {
        private const int TopAnimeCount = 6;
        private const int UpdatesCount = 5;
        private readonly DataManager _dataManager;

        public HomeController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Anime> list = await _dataManager.Anime.GetAnimeAsync();

            HomeIndexViewModel model = new HomeIndexViewModel
            {
                TopAnime = HelperDTO.TransformAnime(list
                    .OrderByDescending(x => x.Rating)
                    .Take(TopAnimeCount)),
                Updates = HelperDTO.TransformAnime(list
                    .OrderByDescending(x => x.Id)
                    .Take(UpdatesCount))
            };

            return View(model);
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}
