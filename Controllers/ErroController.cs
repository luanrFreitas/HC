using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle.Controllers
{
    public class ErroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
