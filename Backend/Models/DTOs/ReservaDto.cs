using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class ReservaDTO
    {
        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdPaseador { get; set; }

        [Required]
        public int IdServicio { get; set; }

        [Required]
        public int IdPerro { get; set; }

        [Required]
        public int IdHorario { get; set; }

        [Required]
        public DateTime FechaReserva { get; set; }

    }

}
