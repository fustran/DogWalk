using Models.Context;
using Models.Interfaces.IRepositorio;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repositorio
{
    public class ServicioRepositorio : Repositorio<Servicio>, IServicioRepositorio
    {
        private readonly DogWalkPlusContext _context;

        public ServicioRepositorio(DogWalkPlusContext context) : base(context)
        {
            _context = context;
        }

        public void Actualizar(Servicio servicio)
        {
            var servicioDb = _context.Servicios.FirstOrDefault(s => s.IdServicio == servicio.IdServicio);
            if (servicioDb != null)
            {
                servicioDb.NombreServicio = servicio.NombreServicio;
                servicioDb.DescripcionServicio = servicio.DescripcionServicio;
                _context.SaveChanges();
            }
        }
    }
}
