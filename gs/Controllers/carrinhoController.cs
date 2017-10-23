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
    public class carrinhoController : Controller
    {

        gsEntities db = new gsEntities();
        // GET: carrinho
        public ActionResult Index()
        {
            HttpCookie gshopCookie = Request.Cookies["mycookiegs"];
            //decimal calculoTotalPedido = 0;
            if (gshopCookie != null)
            {
                string tkn = gshopCookie.Value.ToString();
                var pedidosCarrinho = db.pedidos.Where(t => t.token == tkn).FirstOrDefault();
                if (pedidosCarrinho != null)
                {
                    ViewBag.itensPedidoCarrinho = (from ip in db.itens_pedido where ip.pedidos.id == pedidosCarrinho.id select ip).ToList();
                    //foreach (var itemPedido in itensPedidoCarrinho)
                    //{
                    //    calculoTotalPedido = calculoTotalPedido + (itemPedido.preco * itemPedido.quantidade);
                    //}
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
    }
}