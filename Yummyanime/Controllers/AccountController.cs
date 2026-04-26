using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Domain.Entities;
using Yummyanime.Infrastructure;
using Yummyanime.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Yummyanime.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataManager _dataManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            DataManager dataManager,
            IWebHostEnvironment env)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dataManager = dataManager;
            _env = env;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            SignInResult result =
                await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, model.RememberMe, false);

            if (result.Succeeded)
                return Redirect(returnUrl ?? "/");

            ModelState.AddModelError(string.Empty, "Неверный логин и пароль");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? avatarFile, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            string? avatarFileName = await SaveAvatarAsync(avatarFile);

            AppUser user = new AppUser
            {
                UserName = model.UserName,
                DisplayName = model.DisplayName,
                Email = model.Email,
                EmailConfirmed = true,
                AvatarFileName = avatarFileName
            };

            IdentityResult createResult = await _userManager.CreateAsync(user, model.Password!);
            if (!createResult.Succeeded)
            {
                foreach (IdentityError error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Redirect(returnUrl ?? Url.Action("Profile", "Account")!);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            AppUser? user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToAction("Login");
            }

            IReadOnlyCollection<Anime> favorites = await _dataManager.UserAnimeFavorites.GetAnimeByUserAndStatusAsync(user.Id, UserAnimeListStatus.Favorite);
            IReadOnlyCollection<Anime> watching = await _dataManager.UserAnimeFavorites.GetAnimeByUserAndStatusAsync(user.Id, UserAnimeListStatus.Watching);
            IReadOnlyCollection<Anime> planned = await _dataManager.UserAnimeFavorites.GetAnimeByUserAndStatusAsync(user.Id, UserAnimeListStatus.Planned);
            IReadOnlyCollection<Anime> completed = await _dataManager.UserAnimeFavorites.GetAnimeByUserAndStatusAsync(user.Id, UserAnimeListStatus.Completed);
            IReadOnlyCollection<Anime> paused = await _dataManager.UserAnimeFavorites.GetAnimeByUserAndStatusAsync(user.Id, UserAnimeListStatus.Paused);

            ProfileViewModel model = new ProfileViewModel
            {
                UserName = user.UserName ?? string.Empty,
                DisplayName = user.DisplayName ?? (user.UserName ?? string.Empty),
                Email = user.Email ?? string.Empty,
                AvatarFileName = user.AvatarFileName,
                FavoriteAnime = HelperDTO.TransformAnime(favorites).ToList(),
                WatchingAnime = HelperDTO.TransformAnime(watching).ToList(),
                PlannedAnime = HelperDTO.TransformAnime(planned).ToList(),
                CompletedAnime = HelperDTO.TransformAnime(completed).ToList(),
                PausedAnime = HelperDTO.TransformAnime(paused).ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task<string?> SaveAvatarAsync(IFormFile? avatarFile)
        {
            if (avatarFile is null || avatarFile.Length == 0)
            {
                return null;
            }

            string uploadsDir = Path.Combine(_env.WebRootPath, "img", "avatars");
            Directory.CreateDirectory(uploadsDir);

            string ext = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
            string fileName = $"avatar-{Guid.NewGuid():N}{ext}";
            string filePath = Path.Combine(uploadsDir, fileName);

            await using FileStream fs = new(filePath, FileMode.Create);
            await avatarFile.CopyToAsync(fs);

            return fileName;
        }
    }
}
