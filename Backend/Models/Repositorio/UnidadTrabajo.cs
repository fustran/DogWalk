using Models.Context;
using Models.Interfaces.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly DogWalkPlusContext _context;
        public IServicioRepositorio Servicio { get; private set; }

       

        public UnidadTrabajo(DogWalkPlusContext context)
        {
            _context = context;
            Servicio = new ServicioRepositorio(context);

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }
    }
}
