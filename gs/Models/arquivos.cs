//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace gs.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class arquivos
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string slug { get; set; }
        public string descricao { get; set; }
        public Nullable<System.DateTime> data_criacao { get; set; }
        public Nullable<System.DateTime> data_alteracao { get; set; }
        public Nullable<int> ordem { get; set; }
        public string filename { get; set; }
        public Nullable<int> status { get; set; }
        public string secao { get; set; }
        public int referencia_id { get; set; }
        public string tipo { get; set; }
    }
}
