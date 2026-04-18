using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Domain.Entities;
using Yummyanime.Infrastructure;
using Yummyanime.Models;

namespace Yummyanime.Controllers
{
    public class AnimeController : Controller
    {
        private readonly DataManager _dataManager;
        private const int PageSize = 12;

        public AnimeController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<IActionResult> Index(string? search, string sort = "year_desc", int page = 1)
        {
            IEnumerable<Anime> list = await _dataManager.Anime.GetAnimeAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                list = list.Where(x => !string.IsNullOrWhiteSpace(x.Title) &&
                    x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            list = sort switch
            {
                "year_asc" => list.OrderBy(x => x.Year),
                "rating_desc" => list.OrderByDescending(x => x.Rating),
                "rating_asc" => list.OrderBy(x => x.Rating),
                _ => list.OrderByDescending(x => x.Year)
            };

            int totalItems = list.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            if (totalPages == 0)
            {
                totalPages = 1;
            }

            page = Math.Clamp(page, 1, totalPages);

            IEnumerable<Anime> pagedItems = list
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            AnimeCatalogViewModel model = new AnimeCatalogViewModel
            {
                Items = HelperDTO.TransformAnime(pagedItems),
                Search = search,
                Sort = sort,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }

        public async Task<IActionResult> Show(int id)
        {
            Anime? entity = await _dataManager.Anime.GetAnimeByIdAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            AnimeDTO entityDTO = HelperDTO.TransformAnime(entity);
            return View(entityDTO);
        }
    }
}
