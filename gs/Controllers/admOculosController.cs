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
    public class admOculosController : Controller
    {
        gsEntities db = new gsEntities();
        HelpController ctrl = new HelpController();
        // GET: admOculos
        public ActionResult Index()
        {
            var c = db.oculos.ToList();
            return View(c);
        }

        public ActionResult Create()
        {
            ViewBag.estilo_de_oculos = (from ct in db.categorias where ct.secao == "estilo_de_oculos" orderby ct.titulo ascending select ct).ToList();
            ViewBag.cor_da_armacao = (from ct in db.categorias where ct.secao == "cor_da_armacao" orderby ct.titulo ascending select ct).ToList();
            ViewBag.tipo_de_lente = (from ct in db.categorias where ct.secao == "tipo_de_lente" orderby ct.titulo ascending select ct).ToList();
            ViewBag.marcas = (from m in db.marcas orderby m.titulo ascending select m).ToList();
            var c = new oculos();
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(oculos consulta, IEnumerable<HttpPostedFileBase> files)
        {
            consulta.data_criacao = DateTime.Now;
            consulta.data_alteracao = null;
            consulta.preco = Convert.ToDecimal(consulta.preco);
            if (consulta.titulo != null)
                consulta.slug = ctrl.RemoveDiacritics(consulta.titulo);

            //Preenche o campo Ordem da tabela de Categorias.
            var ultimoitem = new oculos();
            var consultaOrdem = (from ev in db.oculos orderby ev.ordem ascending select ev).ToList();
            if (consultaOrdem.Count > 0)
            {
                ultimoitem = consultaOrdem[consultaOrdem.Count - 1];
                consulta.ordem = ultimoitem.ordem + 1;
            }
            else
                consulta.ordem = 1;

            consulta.masculino = Convert.ToInt32(Request.Form["masculino"]);
            consulta.feminino = Convert.ToInt32(Request.Form["feminino"]);
            consulta.infantil = Convert.ToInt32(Request.Form["infantil"]);
            consulta.marca = Convert.ToInt32(Request.Form["marca"]);

            //Cadastra a Categoria.
            if (ModelState.IsValid)
            {
                //Save Post                
                db.oculos.Add(consulta);
                db.SaveChanges();

                if (!String.IsNullOrEmpty(Request.Form["estilo_de_oculos"]))
                {
                    int estilo_oculos = Convert.ToInt32(Request.Form["estilo_de_oculos"]);
                    var oculos_estilos = new oculos_estilos();
                    oculos_estilos.estilo_id = estilo_oculos;
                    oculos_estilos.oculos_id = consulta.id;
                    db.oculos_estilos.Add(oculos_estilos);
                    db.SaveChanges();
                }

                if (!String.IsNullOrEmpty(Request.Form["tipo_de_lente"]))
                {
                    int tipo_lente = Convert.ToInt32(Request.Form["tipo_de_lente"]);
                    var oculos_lentes = new oculos_lentes();
                    oculos_lentes.lente_id = tipo_lente;
                    oculos_lentes.oculos_id = consulta.id;
                    db.oculos_lentes.Add(oculos_lentes);
                    db.SaveChanges();
                }

                if (!String.IsNullOrEmpty(Request.Form["cor_da_armacao"]))
                {
                    string s = Request.Form["cor_da_armacao"];
                    string[] cores = s.Split(',');
                    foreach (string word in cores)
                    {
                        var oculos_armacao = new oculos_armacoes();
                        oculos_armacao.armacao_id = Convert.ToInt32(word);
                        oculos_armacao.oculos_id = consulta.id;
                        db.oculos_armacoes.Add(oculos_armacao);
                        db.SaveChanges();
                    }
                }

                TempData["acao"] = "dados inseridos com sucesso";
            }

            if (files.Count() > 0)
                uploadImagem(consulta, files);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string s)
        {
            var consulta = db.oculos.Single(a => a.id == id);
            ViewBag.estilo_de_oculos = (from ct in db.categorias where ct.secao == "estilo_de_oculos" orderby ct.titulo ascending select ct).ToList();
            ViewBag.estilos_selecionados = db.oculos_estilos.Where(t => t.oculos_id == id).ToList();

            ViewBag.cor_da_armacao = (from ct in db.categorias where ct.secao == "cor_da_armacao" orderby ct.titulo ascending select ct).ToList();
            ViewBag.armacoes_selecionadas = db.oculos_armacoes.Where(t => t.oculos_id == id).ToList();

            ViewBag.tipo_de_lente = (from ct in db.categorias where ct.secao == "tipo_de_lente" orderby ct.titulo ascending select ct).ToList();
            ViewBag.tipos_selecionados = db.oculos_lentes.Where(t => t.oculos_id == id).ToList();

            ViewBag.marcas = (from m in db.marcas orderby m.titulo ascending select m).ToList();

            ViewBag.imgs = (from ct in db.arquivos where ct.referencia_id == id orderby ct.ordem ascending select ct).ToList();

            return View(consulta);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(int id, IEnumerable<HttpPostedFileBase> files)
        {
            var consulta = db.oculos.Single(a => a.id == id);
            //ViewBag.imagens = (from ev in storeDB.imagens where ev.subcategoria_id == id orderby ev.ordem ascending select ev).ToList();
            consulta.data_alteracao = DateTime.Now;
            consulta.preco = Convert.ToDecimal(consulta.preco);
            consulta.slug = ctrl.RemoveDiacritics(consulta.titulo);
            consulta.masculino = Convert.ToInt32(Request.Form["masculino"]);
            consulta.feminino = Convert.ToInt32(Request.Form["feminino"]);
            consulta.infantil = Convert.ToInt32(Request.Form["infantil"]);
            consulta.marca = Convert.ToInt32(Request.Form["marca"]);
            if (TryUpdateModel(consulta))
            {
                db.SaveChanges();

                // -----------------------------  remove todos os relacionamentos anteriores  ----------------------------------
                var consulta_estilos = db.oculos_estilos.Where(t => t.oculos_id == id).ToList();
                foreach (var ce in consulta_estilos)
                {
                    db.oculos_estilos.Remove(ce);
                    db.SaveChanges();
                }

                var consulta_armacoes = db.oculos_armacoes.Where(t => t.oculos_id == id).ToList();
                foreach (var ca in consulta_armacoes)
                {
                    db.oculos_armacoes.Remove(ca);
                    db.SaveChanges();
                }

                var consulta_tipos = db.oculos_lentes.Where(t => t.oculos_id == id).ToList();
                foreach (var ct in consulta_tipos)
                {
                    db.oculos_lentes.Remove(ct);
                    db.SaveChanges();
                }

                // -----------------------------  adiciona novos relacionamentos  ----------------------------------
                if (!String.IsNullOrEmpty(Request.Form["estilo_de_oculos"]))
                {
                    int estilo_oculos = Convert.ToInt32(Request.Form["estilo_de_oculos"]);
                    var oculos_estilos = new oculos_estilos();
                    oculos_estilos.estilo_id = estilo_oculos;
                    oculos_estilos.oculos_id = consulta.id;
                    db.oculos_estilos.Add(oculos_estilos);
                    db.SaveChanges();
                }

                if (!String.IsNullOrEmpty(Request.Form["tipo_de_lente"]))
                {
                    int tipo_lente = Convert.ToInt32(Request.Form["tipo_de_lente"]);
                    var oculos_lentes = new oculos_lentes();
                    oculos_lentes.lente_id = tipo_lente;
                    oculos_lentes.oculos_id = consulta.id;
                    db.oculos_lentes.Add(oculos_lentes);
                    db.SaveChanges();
                }

                if (!String.IsNullOrEmpty(Request.Form["cor_da_armacao"]))
                {
                    string s = Request.Form["cor_da_armacao"];
                    string[] cores = s.Split(',');
                    foreach (string word in cores)
                    {
                        var oculos_armacao = new oculos_armacoes();
                        oculos_armacao.armacao_id = Convert.ToInt32(word);
                        oculos_armacao.oculos_id = consulta.id;
                        db.oculos_armacoes.Add(oculos_armacao);
                        db.SaveChanges();
                    }
                }
                TempData["acao"] = "dados alterados com sucesso";
            }
            else
            {
                return View(consulta);
            }

            if (files.Count() > 0)
                uploadImagem(consulta, files);


            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id, string s)
        {
            try
            {
                var consulta = db.oculos.Single(a => a.id == id);
                db.oculos.Remove(consulta);
                db.SaveChanges();

                var consulta_estilos = db.oculos_estilos.Where(t => t.oculos_id == id).ToList();
                foreach (var ce in consulta_estilos)
                {
                    db.oculos_estilos.Remove(ce);
                    db.SaveChanges();
                }

                var consulta_armacoes = db.oculos_armacoes.Where(t => t.oculos_id == id).ToList();
                foreach (var ca in consulta_armacoes)
                {
                    db.oculos_armacoes.Remove(ca);
                    db.SaveChanges();
                }

                var consulta_tipos = db.oculos_lentes.Where(t => t.oculos_id == id).ToList();
                foreach (var ct in consulta_tipos)
                {
                    db.oculos_lentes.Remove(ct);
                    db.SaveChanges();
                }

                var consulta_imgs = db.arquivos.Where(t => t.referencia_id == id).ToList();
                foreach (var ct in consulta_imgs)
                {
                    db.arquivos.Remove(ct);
                    db.SaveChanges();
                }

                TempData["acao"] = "dados removidos com sucesso";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteImg(int id, string s)
        {
            try
            {
                var consulta = db.arquivos.Single(a => a.id == id);
                db.arquivos.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "imagem removida com sucesso";
                return RedirectToAction("Edit/" + consulta.referencia_id);
            }
            catch
            {
                return View();
            }
        }

        private void uploadImagem(oculos consulta, IEnumerable<HttpPostedFileBase> files)
        {
            var ultimoitem2 = new arquivos();
            var consulta2 = new arquivos();
            foreach (var file in files)
            {
                if (file != null)
                {
                    var consultaOrdem2 = (from ev in db.arquivos where ev.referencia_id == consulta.id orderby ev.ordem ascending select ev).ToList();

                    if (consultaOrdem2.Count() > 0)
                    {
                        ultimoitem2 = consultaOrdem2[(consultaOrdem2.Count()) - 1];
                        consulta2.ordem = ultimoitem2.ordem + 1;
                    }
                    else
                        consulta2.ordem = 1;

                    int id = Convert.ToInt32(consulta.id);
                    //nome do arquivo original
                    var fileName = Path.GetFileName(file.FileName);

                    //atribuicao da extensão do arquivo (.jpg / .gif) na variavel
                    var extensao = Path.GetExtension(fileName);

                    //geracao de numeros randômicos que serao o novo nome do arquivo
                    Random randNum = new Random();
                    var random = randNum.Next().ToString();

                    //mapeamento do caminho absoluto onde ficarão os arquivos
                    string dir = (Server.MapPath("~/uploads/"));

                    //verifica se o diretorio referente ao determinado conteudo existe. caso não exista, cria o diretorio.
                    if (System.IO.Directory.Exists(dir) == false)
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }

                    var path = Path.Combine(Server.MapPath("~/uploads/"), random + extensao);
                    file.SaveAs(path);

                    //salva na tabela Novidades / coluna Imagem o nome final do arquivo.
                    consulta2.filename = random + extensao;
                    //consulta2.titulo = "";
                    //consulta2.slug = "";
                    consulta2.status = 1; //Convert.ToInt32(Request.Form["status_img"]);
                    consulta2.referencia_id = consulta.id;
                    consulta2.data_alteracao = null;
                    consulta2.data_criacao = DateTime.Now;
                    consulta2.secao = "oculos";
                    consulta2.tipo = "imagem";
                    try
                    {
                        db.arquivos.Add(consulta2);
                        db.SaveChanges();
                    }
                    catch (Exception exception)
                    {

                    }
                }
                /*
                else
                {
                    consulta2.legenda = Request.Form["legenda"];
                    consulta2.alt = Request.Form["alt"];
                    if (TryUpdateModel(consulta2))
                    {
                        storeDB.SaveChanges();
                    }
                }
                */
            }
        }
    }
}