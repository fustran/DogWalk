using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Context;
using Models.Models;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpinionController : BaseApiController
    {
        private readonly DogWalkPlusContext _context;

        public OpinionController(DogWalkPlusContext context)
        {
            _context = context;
        }

        [HttpGet("opiniones")]
        public async Task<ActionResult<IEnumerable<Opinione>>> GetOpiniones()
        {
            return await _context.Opiniones.ToListAsync();
        }

        [HttpGet("opiniones/{idOpinion}")]
        public async Task<ActionResult<Opinione>> GetOpinione(int idOpinion)
        {
            var opinion = await _context.Opiniones.FindAsync(idOpinion);

            if (opinion == null)
            {
                return NotFound();
            }

            return opinion;
        }

        [HttpPost("opiniones")]
        public async Task<ActionResult<Opinione>> PostOpinione(Opinione opinion)
        {
            _context.Opiniones.Add(opinion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOpinione", new { idOpinion = opinion.IdOpinion }, opinion);
        }

        [HttpPut("opiniones/{idOpinion}")]
        public async Task<IActionResult> PutOpinione(int idOpinion, Opinione opinion)
        {
            if (idOpinion != opinion.IdOpinion)
            {
                return BadRequest();
            }

            _context.Entry(opinion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpinioneExists(idOpinion))
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

        [HttpDelete("opiniones/{idOpinion}")]
        public async Task<IActionResult> DeleteOpinione(int idOpinion)
        {
            var opinion = await _context.Opiniones.FindAsync(idOpinion);
            if (opinion == null)
            {
                return NotFound();
            }

            _context.Opiniones.Remove(opinion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OpinioneExists(int idOpinion)
        {
            return _context.Opiniones.Any(e => e.IdOpinion == idOpinion);
        }

    }
}
