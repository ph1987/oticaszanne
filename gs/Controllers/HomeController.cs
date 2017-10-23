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
    public class HomeController : Controller
    {
        gsEntities db = new gsEntities();
        public ActionResult Index()
        {
            IQueryable<oculos> query = db.oculos.Select(c => c);
            if (Request.QueryString["estilo"] != null)
            {
                string estilo = Request.QueryString["estilo"].ToString();
                try
                {
                    var catFiltro = db.categorias.Where(t => t.titulo == estilo).SingleOrDefault();
                    query = from o in query
                            join oe in db.oculos_estilos.Where(e => catFiltro.id == e.estilo_id)
                            on o.id equals oe.oculos_id
                            select o;
                }
                catch
                {

                }
            }
            if (Request.QueryString["genero"] != null)
            {
                string genero = Request.QueryString["genero"].ToString();
                try
                {
                    if (genero == "masculino")
                    {
                        query = from o in query
                                where o.masculino == 1
                                select o;
                    }
                    else if (genero == "feminino")
                    {
                        query = from o in query
                                where o.feminino == 1
                                select o;
                    }
                }
                catch
                {

                }
            }
            if (Request.QueryString["marca"] != null)
            {
                string marca = Request.QueryString["marca"].ToString();
                var ctMarca = db.marcas.Single(a => a.slug == marca);
                try
                {
                        query = from o in query
                                where o.marca == ctMarca.id
                                select o;
                }
                catch
                {

                }
            }
            ViewBag.glasses = query.Where(t => t.status == 1).OrderBy(t => t.ordem).ToList();
            ViewBag.estilo_oculos = (from o in db.categorias where o.status == 1 && o.secao == "estilo_de_oculos" orderby o.ordem ascending select o).ToList();
            //ViewBag.glassesPics = db.arquivos.Where(t => t.status == 1 && t.secao == "oculos").OrderBy(t => t.ordem).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Index(int idprod, HttpPostedFileBase file)
        {
            //ViewBag.glasses = db.oculos.Where(t => t.status == 1).OrderBy(t => t.ordem).ToList();
            IQueryable<oculos> query = db.oculos.Select(c => c);
            if (Request.QueryString["filtro"] != null)
            {
                string fltr = Request.QueryString["filtro"].ToString();
                try
                {
                    var catFiltro = db.categorias.Where(t => t.titulo == fltr).SingleOrDefault();
                    query = from o in query
                            join oe in db.oculos_estilos.Where(e => catFiltro.id == e.estilo_id)
                            on o.id equals oe.oculos_id
                            select o;
                }
                catch
                {

                }
            }
            ViewBag.glasses = query.Where(t => t.status == 1).OrderBy(t => t.ordem).ToList();
            ViewBag.estilo_oculos = (from o in db.categorias where o.status == 1 && o.secao == "estilo_de_oculos" orderby o.ordem ascending select o).ToList();
            HttpCookie cookie = Request.Cookies["mycookiegs"];
            var pedido = new pedidos();
            if (cookie == null)
            {
                pedido.created = DateTime.Now;
                pedido.updated = DateTime.Now;
                pedido.status = "EM ANDAMENTO";
                pedido.token = "gs_" + DateTime.Now.ToString("HH_mm_ss_tt");
                db.pedidos.Add(pedido);
                db.SaveChanges();
            }
            else
            {
                pedido = db.pedidos.Where(t => t.token == cookie.Value.ToString()).FirstOrDefault();
            }

            /* ------------------------------------------------------------------ */

            var itempedido = new itens_pedido();
            itempedido.oculos_id = idprod;
            string reqPreco = "preco-" + idprod;
            string reqQtd = "qty-" + idprod;
            itempedido.preco = Convert.ToDecimal(Request.Form[reqPreco]);
            itempedido.quantidade = Convert.ToInt32(Request.Form[reqQtd]);
            itempedido.pedido_id = pedido.id;
            db.itens_pedido.Add(itempedido);
            db.SaveChanges();

            /* ------------------------------------------------------------------ */

            if (cookie == null)
            {
                cookie = new HttpCookie("mycookiegs");
                cookie.HttpOnly = false;
                cookie.Value = pedido.token;
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index");
        }

        public ActionResult removeItem(int id)
        {
            try
            {
                var itemPedido = db.itens_pedido.Where(t => t.id == id).FirstOrDefault();
                db.itens_pedido.Remove(itemPedido);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }
    }
}