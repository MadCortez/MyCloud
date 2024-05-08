using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyCloud.ViewModels.Account;
using System.Security.Claims;
using MyCloud.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace MyCloud.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccount _accountService;

        public AccountController(IAccount accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home"); // Перенаправляем на домашнюю страницу
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Register(model);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home"); // Перенаправляем на домашнюю страницу
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(model);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _accountService.GetUsers();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return View(response.Data);
            }
            await Logout();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(long id, string name)
        {
            var response = await _accountService.DeleteUser(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string dirPath = $"wwwroot/Files/{name}";
                System.IO.Directory.Delete(dirPath, true);
                return RedirectToAction("GetUsers");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}