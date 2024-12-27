using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string ImagenUri { get; set; }

        public string NombreCategoria { get; set; }

        public string PrecioFormateado => $"${Precio:F2}";
        public string StockFormateado => $"Stock: {Stock}";

        public string FechaRegistroFormateada => FechaRegistro.ToString("dd/MM/yyyy");
    }
}
