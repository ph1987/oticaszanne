using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gs.Models;
using gs.Controllers.Classes;

namespace gs.Controllers
{
    public class formatosController : Controller
    {
        gsEntities db = new gsEntities();
        HelpController ctrl = new HelpController();
        public ActionResult Index(string s, string t)
        {
            var c = db.formatos.ToList();
            return View(c);
        }

        public ActionResult Create(string s)
        {
            var c = new formatos();
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(formatos consulta, HttpPostedFileBase file)
        {
            consulta.data_criacao = DateTime.Now;
            consulta.data_alteracao = null;

            if (consulta.titulo != null)
                consulta.slug = ctrl.RemoveDiacritics(consulta.titulo);

            var ultimoitem = new formatos();
            var consultaOrdem = (from ev in db.formatos orderby ev.ordem ascending select ev).ToList();
            if (consultaOrdem.Count > 0)
            {
                ultimoitem = consultaOrdem[consultaOrdem.Count - 1];
                consulta.ordem = ultimoitem.ordem + 1;
            }
            else
                consulta.ordem = 1;

            if (ModelState.IsValid)
            {          
                db.formatos.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "dados inseridos com sucesso";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string s)
        {
            var consulta = db.formatos.Single(a => a.id == id);
            /*
            try
            {
                ViewBag.imagens = (from ev in storeDB.imagens where ev.categoria_id == id orderby ev.ordem ascending select ev).ToList();
            }
            catch (Exception ex)
            {
                
            }
            */
            return View(consulta);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(int id, HttpPostedFileBase file)
        {
            var consulta = db.formatos.Single(a => a.id == id);
            //ViewBag.imagens = (from ev in storeDB.imagens where ev.subcategoria_id == id orderby ev.ordem ascending select ev).ToList();
            consulta.data_alteracao = DateTime.Now;

            if (TryUpdateModel(consulta))
            {
                db.SaveChanges();

                TempData["acao"] = "dados alterados com sucesso";
            }
            else
            {
                return View(consulta);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id, string s)
        {
            try
            {
                var consulta = db.formatos.Single(a => a.id == id);
                db.formatos.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "dados removidos com sucesso";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}