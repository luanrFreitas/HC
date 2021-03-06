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
using Microsoft.AspNetCore.Authorization;

namespace HustleCastle.Controllers
{
    [Authorize]
    public class DicasController : Controller
    {
        private readonly SheetsDatabase _sheetsDatabase;

        public DicasController(SheetsDatabase sheetsDatabase)
        {
            _sheetsDatabase = sheetsDatabase;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            IList<Tutorial> tutorials = _sheetsDatabase.GetAll<Tutorial>();
            return View(tutorials);
        }

        [AllowAnonymous]
        public ActionResult Detalhes(int id)
        {
            Tutorial tutorial = _sheetsDatabase.GetByID<Tutorial>(id);
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
            Tutorial tutorial = _sheetsDatabase.GetByID<Tutorial>(id);
            return View("Editar", tutorial);
        }


        [HttpPost]
        public ActionResult Save(Tutorial tutorial)
        {
            if (ModelState.IsValid)
            {
                if (tutorial.ID == 0)
                {
                    tutorial.CreatedAt = DateTime.UtcNow.AddHours(-3).ToString();
                    _sheetsDatabase.Add<Tutorial>(tutorial);
                    Audit audit = new Audit(User.Identity.Name, "Create", nameof(Tutorial), tutorial.Title);
                    _sheetsDatabase.Add<Audit>(audit);
                }
                else
                {
                    _sheetsDatabase.Update<Tutorial>(tutorial);
                    Audit audit = new Audit(User.Identity.Name, "Edit", nameof(Tutorial), tutorial.Title);
                    _sheetsDatabase.Add<Audit>(audit);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Editar), tutorial);

        }

        // GET: TutorialsController/Delete/5
        public ActionResult Excluir(int id)
        {
            _sheetsDatabase.Delete<Tutorial>(id);
            Audit audit = new Audit(User.Identity.Name, nameof(Excluir), nameof(Tutorial), id.ToString());
            _sheetsDatabase.Add<Audit>(audit);
            return RedirectToAction(nameof(Index));
        }

        // POST: TutorialsController/Delete/5
        [HttpPost]
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
