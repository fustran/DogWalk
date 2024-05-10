using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Servicios.Interfaces;
using Models.DTOs;
using Models.Interfaces.IRepositorio;

namespace BLL.Servicios
{
    public class Servicio : IServicio
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IMapper _mapper;

        public Servicio(IUnidadTrabajo unidadTrabajo, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _mapper = mapper;
        }

        public string NombreServicio { get; private set; }
        public string DescripcionServicio { get; private set; }

        public async Task Actualizar(ServicioDto modeloDto)
        {
            try
            {
                var servicioDb = await _unidadTrabajo.Servicio.ObtenerPrimero(e => e.NombreServicio == modeloDto.NombreServicio);
                if (servicioDb != null)
                    throw new TaskCanceledException("El servicio ya existe");

                servicioDb.NombreServicio = modeloDto.NombreServicio;
                servicioDb.DescripcionServicio = modeloDto.DescripcionServicio;
                _unidadTrabajo.Servicio.Actualizar(servicioDb);
                await _unidadTrabajo.Guardar();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServicioDto> Agregar(ServicioDto modeloDto)
        {
            try
            {
                Models.Models.Servicio servicio = new Models.Models.Servicio
                {
                    NombreServicio = modeloDto.NombreServicio,
                    DescripcionServicio = modeloDto.DescripcionServicio
                };
                await _unidadTrabajo.Servicio.Agregar(servicio);
                await _unidadTrabajo.Guardar();

                return _mapper.Map<ServicioDto>(servicio);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<IEnumerable<ServicioDto>> ObtenerTodos()
        {
            try
            {
               var lista = await _unidadTrabajo.Servicio.ObtenerTodos(
                                        orderBy: e => e.OrderBy(e => e.NombreServicio));
               return _mapper.Map<IEnumerable<ServicioDto>>(lista);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Remover(int id)
        {
            try
            {
                var servicioDb = await _unidadTrabajo.Servicio.ObtenerPrimero(e => e.NombreServicio == NombreServicio);
                if (servicioDb == null)
                    throw new TaskCanceledException("El servicio no existe");
                _unidadTrabajo.Servicio.Remover(servicioDb);
                await _unidadTrabajo.Guardar();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
