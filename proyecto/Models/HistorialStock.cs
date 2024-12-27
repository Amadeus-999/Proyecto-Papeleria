using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto.Models
{
    public class HistorialStock
    {
        public int IdHistorial { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; } // 'entrada' o 'salida'
        public string Descripcion { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
