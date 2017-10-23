using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using System.Web.WebPages;

namespace gs.Controllers.Classes
{
    public class HelpController
    {
        public HelpController() { }

        public string RemoveDiacritics(string stIn)
        {
            //Remove os acentos da letras.
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            string cleanTitle = sb.ToString().ToLower().Replace(" ", "-");
            cleanTitle = cleanTitle.Replace(":", "-");
            cleanTitle = cleanTitle.Replace(".", "-");
            cleanTitle = cleanTitle.Replace(",", "-");
            cleanTitle = cleanTitle.Replace("?", "");
            cleanTitle = cleanTitle.Replace("!", "");
            cleanTitle = cleanTitle.Replace("@", "");
            cleanTitle = cleanTitle.Replace("#", "");
            cleanTitle = cleanTitle.Replace("$", "");
            //Removes invalid character like .,-_ etc
            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(cleanTitle);
            var encode = System.Text.Encoding.UTF8.GetString(bytes);
            string Slug = encode;

            return (Slug);
        }

        public void addLog(string titulo)
        {
            /*
            var userid = Convert.ToInt32(Session["adm"]);
            if (userid > 0)
            {
                var consultauser = (from us in storeDB.users where us.id == userid select us).Single();
                var log = new logs();
                log.registro = consultauser.nome + " adicionou a categoria " + consulta.titulo + " em " + consulta.data_criacao;
                log.data_criacao = consulta.data_criacao;

                db.logs.AddObject(log);
                db.SaveChanges();
            }
            */
            return;
        }
    }
}