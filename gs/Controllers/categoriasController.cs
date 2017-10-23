using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gs.Models;
using gs.Controllers.Classes;

namespace gs.Controllers
{
    public class categoriasController : Controller
    {
        gsEntities db = new gsEntities();
        HelpController ctrl = new HelpController();
        // GET: categorias
        public ActionResult Index(string s, string t)
        {
            var c = db.categorias.ToList().Where(sec => sec.secao == s);
            return View(c);
        }

        public ActionResult Create(string s)
        {
            var c = new categorias();
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(categorias consulta, HttpPostedFileBase file)
        {
            string sec = Request.Form["sect"];

            consulta.data_criacao = DateTime.Now;
            consulta.data_alteracao = null;
            consulta.secao = sec;
            //consulta.status = Convert.ToInt32(Request.Form["status"]);

            if (consulta.titulo != null)
                consulta.slug = ctrl.RemoveDiacritics(consulta.titulo);

            //Preenche o campo Ordem da tabela de Categorias.
            var ultimoitem = new categorias();
            var consultaOrdem = (from ev in db.categorias where ev.secao == sec orderby ev.ordem ascending select ev).ToList();
            if (consultaOrdem.Count > 0)
            {
                ultimoitem = consultaOrdem[consultaOrdem.Count - 1];
                consulta.ordem = ultimoitem.ordem + 1;
            }
            else
                consulta.ordem = 1;

            //Cadastra a Categoria.
            if (ModelState.IsValid)
            {
                //Save Post                
                db.categorias.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "dados inseridos com sucesso";
            }

            return RedirectToAction("Index", new { s = sec });
        }

        public ActionResult Edit(int id, string s)
        {
            var consulta = db.categorias.Single(a => a.id == id);
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
            var consulta = db.categorias.Single(a => a.id == id);
            string sec = Request.Form["sect"];
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
            return RedirectToAction("Index", new { s = sec });
        }

        public ActionResult Delete(int id, string s)
        {
            try
            {
                var consulta = db.categorias.Single(a => a.id == id);
                db.categorias.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "dados removidos com sucesso";
                return RedirectToAction("Index", new { s = s });
            }
            catch
            {
                return View();
            }
        }

    }
}