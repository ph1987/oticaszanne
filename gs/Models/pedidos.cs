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
    
    public partial class pedidos
    {
        public pedidos()
        {
            this.itens_pedido = new HashSet<itens_pedido>();
        }
    
        public int id { get; set; }
        public string token { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
        public string status { get; set; }
    
        public virtual ICollection<itens_pedido> itens_pedido { get; set; }
    }
}
