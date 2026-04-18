using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Domain.Entities;
using Yummyanime.Infrastructure;
using Yummyanime.Models;

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
            IEnumerable<Anime> list = await _dataManager.Anime.GetAnimeAsync();

            HomeIndexViewModel model = new HomeIndexViewModel
            {
                TopAnime = HelperDTO.TransformAnime(list
                    .OrderByDescending(x => x.Rating)
                    .ThenBy(x => x.Title)
                    .Take(6)),
                Updates = HelperDTO.TransformAnime(list
                    .OrderByDescending(x => x.Id)
                    .Take(5))
            };

            return View(model);
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}
