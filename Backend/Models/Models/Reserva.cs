using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

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

    [Required]
    public string EstadoReserva { get; set; }

    public virtual Horario IdHorarioNavigation { get; set; }

    public virtual Paseador IdPaseadorNavigation { get; set; }

    public virtual Perro IdPerroNavigation { get; set; }

    public virtual Servicio IdServicioNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
