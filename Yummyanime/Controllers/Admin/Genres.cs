using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain.Entities;

namespace Yummyanime.Controllers.Admin
{
    public partial class AdminController
    {
        public async Task<IActionResult> GenresEdit(int id)
        {
            Genre? entity = id == default ? new Genre() : await _dataManager.Genres.GetGenreByIdAsync(id);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> GenresEdit(Genre entity)
        {
            if (!ModelState.IsValid)
            {
                return View(entity);
            }

            await _dataManager.Genres.SaveGenreAsync(entity);
            _logger.LogInformation($"Добавлен/обновлен жанр с ID: {entity.Id}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GenresDelete(int id)
        {
            await _dataManager.Genres.DeleteGenreAsync(id);
            _logger.LogInformation($"Удален жанр с ID: {id}");
            return RedirectToAction("Index");
        }
    }
}
