using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain.Entities;

namespace Yummyanime.Controllers.Admin
{
    public partial class AdminController
    {
        public async Task<IActionResult> AnimeEdit(int id)
        {
            Anime? entity = id == default
                ? new Anime()
                : await _dataManager.Anime.GetAnimeByIdAsync(id);

            ViewBag.Genres = await _dataManager.Genres.GetGenresAsync();
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> AnimeEdit(Anime entity, IFormFile? titleImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = await _dataManager.Genres.GetGenresAsync();
                return View(entity);
            }

            if (titleImageFile != null)
            {
                entity.Photo = await SaveImg(titleImageFile);
            }

            await _dataManager.Anime.SaveAnimeAsync(entity);
            _logger.LogInformation($"Добавлено/обновлено аниме с ID: {entity.Id}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AnimeDelete(int id)
        {
            await _dataManager.Anime.DeleteAnimeAsync(id);
            _logger.LogInformation($"Удалено аниме с ID: {id}");
            return RedirectToAction("Index");
        }
    }
}
