using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCloud.Data;
using MyCloud.Models;
using System.Diagnostics;
using System.IO;
using MyCloud.Helpers;
using Microsoft.AspNetCore.Hosting.Server;

namespace MyCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;

        ApplicationDbContext db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _logger = logger;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(GetFilesHelper.GetUserFiles($"wwwroot/Files/{User.Identity.Name}"));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = $"/Files/{User.Identity.Name}/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileData file = new FileData { FileName = uploadedFile.FileName, Path = path };
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            string filePath = $"/Files/{User.Identity.Name}/" + fileName;
            return File(filePath, "application/octet-stream", fileName);
        }
    }
}