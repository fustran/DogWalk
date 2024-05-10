using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Context;
using Models.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Models.DTOs;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : BaseApiController
    {
        private readonly DogWalkPlusContext _context;

        public ReservaController(DogWalkPlusContext context)
        {
            _context = context;
        }

        [HttpGet("reservas")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas.ToListAsync();
        }

        [HttpGet("reservas/{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(ReservaDTO reservaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reserva = MapToReserva(reservaDto);

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserva", new { id = reserva.IdReserva }, reserva);
        }


        private Reserva MapToReserva(ReservaDTO reservaDto)
        {
            return new Reserva
            {
                IdUsuario = reservaDto.IdUsuario,
                IdPaseador = reservaDto.IdPaseador,
                IdServicio = reservaDto.IdServicio,
                IdPerro = reservaDto.IdPerro,
                IdHorario = reservaDto.IdHorario,
                FechaReserva = reservaDto.FechaReserva,
            };
        }


        [HttpPut("reservas/{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.IdReserva)
            {
                return BadRequest();
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        [HttpDelete("reservas/{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.IdReserva == id);
        }

    }
}
