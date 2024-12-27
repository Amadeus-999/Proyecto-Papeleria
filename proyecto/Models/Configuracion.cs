using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto.Models
{
    public class Configuracion
    {
        public int IdConfig { get; set; }
        public string Clave { get; set; }
        public string Valor { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }

}
