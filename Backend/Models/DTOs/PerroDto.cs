using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PerroDto
    {
        public string Nombre { get; set; }

        public string Raza { get; set; }

        public int Edad { get; set; }

        public int IdUsuario { get; set; }

        public string Instagram { get; set; }

        public string Tiktok { get; set; }
    }
}
