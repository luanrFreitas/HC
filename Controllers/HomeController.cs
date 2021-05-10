using HustleCastle.Data;
using HustleCastle.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HustleCastle.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SheetsDatabase sheetsDatabase;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            var a = Path.Combine(_hostingEnvironment.ContentRootPath, "client_secret.json");
            using (var stream = new FileStream(a, FileMode.Open, FileAccess.Read))
            {
                sheetsDatabase = new SheetsDatabase("Teste", "1VKcGHaNzCU6dH9MNMEnlxfcb5sX7fM2t1G8OblestPg", stream);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                User userFromDB = sheetsDatabase.GetAll<User>().Where(X => X.Username == user.Username).FirstOrDefault();


                if (userFromDB != null &&  user.Password == userFromDB.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.Username)
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos");
            }

            return View(user);

        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(User user)
        {
            if (ModelState.IsValid)
            {
                IList<User> users = sheetsDatabase.GetAll<User>();
                if (!users.Any(x => x.Username == user.Username))
                {
                    sheetsDatabase.Add<User>(user);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.Username)
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(String.Empty,"Já existe um usuário com esse nome");
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError(HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Log()
        {
           sheetsDatabase.Delete<User>(5);
            _logger.LogDebug("Luan Acessou o Log");
            return Content("2");
        }
    }
}
