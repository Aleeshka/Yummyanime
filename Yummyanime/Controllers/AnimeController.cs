using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
            string safeSort = sort switch
            {
                "year_asc" => "year_asc",
                "rating_desc" => "rating_desc",
                "rating_asc" => "rating_asc",
                _ => "year_desc"
            };

            if (!string.IsNullOrWhiteSpace(search))
            {
                list = list.Where(x => !string.IsNullOrWhiteSpace(x.Title) &&
                    x.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            list = safeSort switch
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
                Sort = safeSort,
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

            bool isFavorite = false;
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                isFavorite = await _dataManager.UserAnimeFavorites.IsInFavoritesAsync(userId, id);
            }

            ViewBag.IsFavorite = isFavorite;
            AnimeDTO entityDTO = HelperDTO.TransformAnime(entity);
            return View(entityDTO);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int id, string? returnUrl)
        {
            Anime? anime = await _dataManager.Anime.GetAnimeByIdAsync(id);
            if (anime is null)
            {
                return NotFound();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            bool isFavorite = await _dataManager.UserAnimeFavorites.IsInFavoritesAsync(userId, id);
            if (isFavorite)
            {
                await _dataManager.UserAnimeFavorites.RemoveAsync(userId, id);
            }
            else
            {
                await _dataManager.UserAnimeFavorites.AddAsync(userId, id);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Show), new { id });
        }
    }
}
