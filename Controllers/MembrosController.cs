﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle.Controllers
{
    public class MembrosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
