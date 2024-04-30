
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : BaseApiController
    {
        private readonly DogWalkPlusContext _context;

        public ServicioController(DogWalkPlusContext context)
        {
            _context = context;
        }

        [HttpGet("servicios")] //GET api/servicios Obtenemos todos los servicios
        public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
        {
            return await _context.Servicios.ToListAsync();
        }

        [HttpGet("servicios/{id}")] //GET api/servicios/5 Obtenemos un servicio con un id específico
        public async Task<ActionResult<Servicio>> GetServicio(int idServicio)
        {
            var servicio = await _context.Servicios.FindAsync(idServicio);

            if (servicio == null)
            {
                return NotFound();
            }

            return servicio;
        }

        [HttpPost("servicios")] //POST api/servicios Creamos un nuevo servicio
        public async Task<ActionResult<Servicio>> PostServicio(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicio", new { id = servicio.IdServicio }, servicio);
        }

        [HttpPut("servicios/{id}")] //PUT api/servicios/5 Actualizamos un servicio con un id específico
        public async Task<IActionResult> PutServicio(int idServicio, Servicio servicio)
        {
            if (idServicio != servicio.IdServicio)
            {
                return BadRequest();
            }

            _context.Entry(servicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(idServicio))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("servicios/{id}")] //DELETE api/servicios/5 Eliminamos un servicio con un id específico
        public async Task<IActionResult> DeleteServicio(int idServicio)
        {
            var servicio = await _context.Servicios.FindAsync(idServicio);
            if (servicio == null)
            {
                return NotFound();
            }

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioExists(int idServicio)
        {
            return _context.Servicios.Any(e => e.IdServicio == idServicio);
        }
    }
}
