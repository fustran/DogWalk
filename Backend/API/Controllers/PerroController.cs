using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Context;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerroController : BaseApiController
    {
        private readonly DogWalkPlusContext _context;

        public PerroController(DogWalkPlusContext context)
        {
            _context = context;
        }

        [HttpGet("perros")] //GET api/perros Obtenemos todos los perros
        public async Task<ActionResult<IEnumerable<Perro>>> GetPerros()
        {
            return await _context.Perros.ToListAsync();
        }

        [HttpGet("perros/{id}")] //GET api/perros/5 Obtenemos un perro con un id específico
        public async Task<ActionResult<Perro>> GetPerro(int idPerro)
        {
            var perro = await _context.Perros.FindAsync(idPerro);

            if (perro == null)
            {
                return NotFound();
            }

            return perro;
        }

        [HttpPost("perros")] //POST api/perros Creamos un nuevo perro
        public async Task<ActionResult<Perro>> Registro(PerroDto perroDto)
        {
            if(await PerroExiste(perroDto.Instagram))
            {
                return BadRequest("El perro ya existe");
            }

            var perro = new Perro
            {
                Nombre = perroDto.Nombre,
                Raza = perroDto.Raza,
                Edad = perroDto.Edad,
                IdUsuario = perroDto.IdUsuario,
                Instagram = perroDto.Instagram,
                Tiktok = perroDto.Tiktok
            };

            _context.Perros.Add(perro);
            await _context.SaveChangesAsync();

            return Ok(perro);
        }


        private async Task<bool> PerroExiste(string instagram)
        {
            return await _context.Perros.AnyAsync(e => e.Instagram == instagram);
        }

        [HttpPut("perros/{id}")] //PUT api/perros/5 Actualizamos un perro con un id específico
        public async Task<IActionResult> PutPerro(int idPerro, Perro perro)
        {
            if (idPerro != perro.IdPerro)
            {
                return BadRequest();
            }

            _context.Entry(perro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerroExists(idPerro))
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

        [HttpDelete("perros/{id}")] //DELETE api/perros/5 Eliminamos un perro con un id específico
        public async Task<IActionResult> DeletePerro(int idPerro)
        {
            var perro = await _context.Perros.FindAsync(idPerro);
            if (perro == null)
            {
                return NotFound();
            }

            _context.Perros.Remove(perro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerroExists(int idPerro)
        {
            return _context.Perros.Any(e => e.IdPerro == idPerro);
        }

    }
}
