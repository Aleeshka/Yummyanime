using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Infrastructure;
using Yummyanime.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Yummyanime.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataManager _dataManager;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            DataManager dataManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dataManager = dataManager;
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
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            IdentityUser user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
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
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToAction("Login");
            }

            var favoriteAnime = await _dataManager.UserAnimeFavorites.GetFavoriteAnimeByUserIdAsync(user.Id);

            ProfileViewModel model = new ProfileViewModel
            {
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FavoriteAnime = HelperDTO.TransformAnime(favoriteAnime).ToList()
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
    }
}
