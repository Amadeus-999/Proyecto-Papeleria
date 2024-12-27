using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto.Models
{
    public class Pedidos
    {
        public int IdPedido { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } // 'pendiente', 'completado', 'cancelado'
        public decimal Total { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
