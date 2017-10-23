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
    public class checkoutController : Controller
    {
        gsEntities db = new gsEntities();
        // GET: checkout
        public ActionResult Index()
        {
            HttpCookie gshopCookie = Request.Cookies["mycookiegs"];
            if (gshopCookie != null)
            {
                string tkn = gshopCookie.Value.ToString();
                var pedidosCarrinho = db.pedidos.Where(t => t.token == tkn).FirstOrDefault();
                if (pedidosCarrinho != null)
                {
                    ViewBag.itensPedidoCarrinho = (from ip in db.itens_pedido where ip.pedidos.id == pedidosCarrinho.id select ip).ToList();
                }
            }
            return View();
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

        [HttpPost]
        public ActionResult Create(clientes consulta)
        {
            var c = db.clientes.ToList();
            foreach(var clientes in c)
            {
                if (clientes.email == Request.Form["email"])
                {
                    TempData["acao"] = "E-mail já cadastrado";
                    return RedirectToAction("Index");
                }
            }
            consulta.created = DateTime.Now;
            consulta.updated = null;
            consulta.status = 1;
            consulta.newsletter = 1;
            consulta.rua = Request.Form["rua"];
            consulta.bairro = Request.Form["bairro"];
            consulta.estado = Request.Form["uf"];
            consulta.cidade = Request.Form["cidade"];
            if (ModelState.IsValid)
            {
                db.clientes.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "Cadastro realizado com sucesso";
            }

            return RedirectToAction("Index");
        }
    }
}