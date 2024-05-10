using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Context;
using Models.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : BaseApiController
    {
        private readonly DogWalkPlusContext _context;

        public RankingController(DogWalkPlusContext context)
        {
            _context = context;
        }

        [HttpGet("rankings")]
        public async Task<ActionResult<IEnumerable<RankingDto>>> GetRankings()
        {
            var rankings = await _context.Rankings
                .Include(r => r.IdPaseadorNavigation)
                .ToListAsync();

            var rankingDtos = rankings.Select(r => new RankingDto
            {
                NombrePaseador = r.IdPaseadorNavigation.Nombre, // Asume que tu entidad Paseador tiene una propiedad Nombre
                Comentario = r.Comentario,
                Valoracion = r.Valoracion
            }).ToList();

            return rankingDtos;
        }


       
        /*Este método filtra los rankings por valoración pero me da error la API
        [HttpGet("rankings")] 
        public async Task<ActionResult<IEnumerable<Ranking>>> GetRankings(int? valoracion = null)
        {
            IQueryable<Ranking> query = _context.Rankings;

            if (valoracion.HasValue)
            {
                query = query.Where(r => r.Valoracion == valoracion.Value);
            }

            return await query.ToListAsync();
        }
        */

        [HttpPost("rankings")]
        public async Task<ActionResult<Ranking>> PostRanking(Ranking ranking)
        {
            _context.Rankings.Add(ranking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRanking", new { idUsuario = ranking.IdUsuario, idPaseador = ranking.IdPaseador }, ranking);
        }

        [HttpPut("rankings/{idUsuario}/{idPaseador}")]
        public async Task<IActionResult> PutRanking(int idUsuario, int idPaseador, Ranking ranking)
        {
            if (idUsuario != ranking.IdUsuario || idPaseador != ranking.IdPaseador)
            {
                return BadRequest();
            }

            _context.Entry(ranking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RankingExists(idUsuario, idPaseador))
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

        [HttpDelete("rankings/{idUsuario}/{idPaseador}")]
        public async Task<IActionResult> DeleteRanking(int idUsuario, int idPaseador)
        {
            var ranking = await _context.Rankings.FirstOrDefaultAsync(r => r.IdUsuario == idUsuario && r.IdPaseador == idPaseador);
            if (ranking == null)
            {
                return NotFound();
            }

            _context.Rankings.Remove(ranking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RankingExists(int idUsuario, int idPaseador)
        {
            return _context.Rankings.Any(e => e.IdUsuario == idUsuario && e.IdPaseador == idPaseador);
        }

    }
}
