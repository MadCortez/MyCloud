using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyCloud.ViewModels.Account;
using System.Security.Claims;
using MyCloud.Interfaces;

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
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Register(model);
                if (response.StatusCode == Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }
    }
}