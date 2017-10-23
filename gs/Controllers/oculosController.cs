using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gs.Models;
using gs.Controllers.Classes;
using System.IO;

namespace gs.Controllers
{
    public class oculosController : Controller
    {
        gsEntities db = new gsEntities();
        HelpController ctrl = new HelpController();
        // GET: oculos
        public ActionResult Index(string slug, int id)
        {
            var consulta = db.oculos.Single(a => a.id == id);
            ViewBag.estilo_de_oculos = (from ct in db.categorias where ct.secao == "estilo_de_oculos" orderby ct.titulo ascending select ct).ToList();
            ViewBag.estilos_selecionados = db.oculos_estilos.Where(t => t.oculos_id == id).ToList();

            ViewBag.cor_da_armacao = (from ct in db.categorias where ct.secao == "cor_da_armacao" orderby ct.titulo ascending select ct).ToList();
            ViewBag.armacoes_selecionadas = db.oculos_armacoes.Where(t => t.oculos_id == id).ToList();

            ViewBag.tipo_de_lente = (from ct in db.categorias where ct.secao == "tipo_de_lente" orderby ct.titulo ascending select ct).ToList();
            ViewBag.tipos_selecionados = db.oculos_lentes.Where(t => t.oculos_id == id).ToList();

            ViewBag.imgs = (from ct in db.arquivos where ct.referencia_id == id orderby ct.ordem ascending select ct).ToList();

            return View(consulta);
        }
    }
}