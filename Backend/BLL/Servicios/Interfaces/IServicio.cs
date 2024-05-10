using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Servicios.Interfaces
{
    public interface IServicio
    {
        Task<IEnumerable<ServicioDto>> ObtenerTodos();
        Task<ServicioDto> Agregar(ServicioDto modeloDto);
        Task Actualizar(ServicioDto modeloDto);
        Task Remover(int id);
    }
}
