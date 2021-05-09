using HustleCastle.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using HustleCastle.Data;

namespace HustleCastle.Controllers
{
    public class DicasController : BaseController
    {
        private readonly SheetsDatabase sheetsDatabase;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DicasController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var a = Path.Combine(_hostingEnvironment.ContentRootPath, "client_secret.json");
            using (var stream = new FileStream(a, FileMode.Open, FileAccess.Read))
            {
                sheetsDatabase = new SheetsDatabase("Teste", "1VKcGHaNzCU6dH9MNMEnlxfcb5sX7fM2t1G8OblestPg", stream);
            }
        }
        // GET: TutorialsController
        public ActionResult Index()
        {
            IList<Tutorial> tutorials = sheetsDatabase.GetAll<Tutorial>();
            return View(tutorials);
        }

        // GET: TutorialsController/Details/5
        public ActionResult Detalhes(int id)
        {
            Tutorial tutorial = sheetsDatabase.GetByID<Tutorial>(id);
            return View(tutorial);
        }

        // GET: TutorialsController/Create
        public ActionResult Criar()
        {
            Tutorial tutorial = new Tutorial();
            return View("Editar", tutorial);
        }

        // GET: TutorialsController/Edit/5
        public ActionResult Editar(int id)
        {
            Tutorial tutorial = sheetsDatabase.GetByID<Tutorial>(id);
            return View("Editar", tutorial);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Tutorial tutorial)
        {
            try
            {
                if (tutorial.ID == 0)
                {
                    tutorial.CreatedAt = DateTime.Now.ToString();
                    
                    sheetsDatabase.Add<Tutorial>(tutorial);
                }
                else
                {
                    sheetsDatabase.Update<Tutorial>(tutorial);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Editar", tutorial);
            }
        }

        // GET: TutorialsController/Delete/5
        public ActionResult Excluir(int id)
        {
            sheetsDatabase.Delete<Tutorial>(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: TutorialsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
