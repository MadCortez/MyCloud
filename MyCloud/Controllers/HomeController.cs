using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCloud.Data;
using MyCloud.Models;
using System.Diagnostics;
using System.IO;
using MyCloud.Helpers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Authorization;
using System.IO.Compression;
using System.Threading.Tasks;
using MyCloud.Implementations;

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
                ZipHelper.CreateZip(uploadedFile.FileName, User.Identity.Name);
                EncryptionHelper.EncryptFile(uploadedFile.FileName, User.Identity.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            fileName = fileName.Replace("/", "").Replace("\\", "");
            string filePath = $"/Files/{User.Identity.Name}/" + fileName;
            EncryptionHelper.DecryptFile(fileName, User.Identity.Name);
            ZipHelper.ExtractZip(fileName, User.Identity.Name);
            string rootPath = "D:\\MyCloud\\MyCloud\\wwwroot\\";

            var fileStream = new FileStream(rootPath + filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new FileCleanupStreamResult(fileStream, "application/octet-stream", () =>
            {
                try
                {
                    System.IO.File.Delete(rootPath + filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error deleting file: {ex.Message}");
                }
            })
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            string filePath = $"wwwroot/Files/{User.Identity.Name}/" + fileName;
            System.IO.File.Delete(filePath);
            return RedirectToAction("Index");
        }
    }
}