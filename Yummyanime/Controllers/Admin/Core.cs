using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;

namespace Yummyanime.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public partial class AdminController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<AdminController> _logger;

        public AdminController(DataManager dataManager, IWebHostEnvironment hostingEnvironment, ILogger<AdminController> logger)
        {
            _dataManager = dataManager;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Genres = await _dataManager.Genres.GetGenresAsync();
            ViewBag.Anime = await _dataManager.Anime.GetAnimeAsync();
            return View();
        }

        public async Task<string> SaveImg(IFormFile img)
        {
            string uploadsDir = Path.Combine(_hostingEnvironment.WebRootPath, "img");
            Directory.CreateDirectory(uploadsDir);

            string extension = Path.GetExtension(img.FileName).ToLowerInvariant();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(img.FileName).ToLowerInvariant();
            string safeName = Regex.Replace(fileNameWithoutExtension, "[^a-z0-9_-]", "-").Trim('-');
            if (string.IsNullOrWhiteSpace(safeName))
            {
                safeName = "poster";
            }

            string fileName = $"{safeName}-{Guid.NewGuid():N}{extension}";
            string path = Path.Combine(uploadsDir, fileName);

            await using FileStream stream = new FileStream(path, FileMode.Create);
            await img.CopyToAsync(stream);

            return fileName;
        }
    }
}
