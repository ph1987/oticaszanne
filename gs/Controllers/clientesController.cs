using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gs.Models;
using gs.Controllers.Classes;

namespace gs.Controllers
{
    public class clientesController : Controller
    {
        gsEntities db = new gsEntities();
        HelpController ctrl = new HelpController();
        public ActionResult Index(string s, string t)
        {
            var c = db.clientes.ToList();
            return View(c);
        }

        public ActionResult Create(string s)
        {
            var c = new clientes();
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(clientes consulta, HttpPostedFileBase file)
        {
            consulta.created = DateTime.Now;
            if (ModelState.IsValid)
            {          
                db.clientes.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "dados inseridos com sucesso";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string s)
        {
            var consulta = db.clientes.Single(a => a.id == id);
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
            var consulta = db.clientes.Single(a => a.id == id);
            //ViewBag.imagens = (from ev in storeDB.imagens where ev.subcategoria_id == id orderby ev.ordem ascending select ev).ToList();
            consulta.updated = DateTime.Now;

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
                var consulta = db.clientes.Single(a => a.id == id);
                db.clientes.Remove(consulta);
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