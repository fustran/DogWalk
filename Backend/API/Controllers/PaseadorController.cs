using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.DTOs;
using Models.Interfaces;
using Models.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaseadorController : BaseApiController
    {
        private readonly DogWalkPlusContext _context;
        private readonly ITokenServicioPaseador _tokenServicioPaseador;


        public PaseadorController(DogWalkPlusContext context, ITokenServicioPaseador tokenServicioPaseador)
        {
            _context = context;
            _tokenServicioPaseador = tokenServicioPaseador;
        }

        [Authorize]
        [HttpGet("paseadores")]
        public async Task<ActionResult<IEnumerable<Paseador>>> GetPaseadores()
        {
            return await _context.Paseadors.ToListAsync();
        }

        [HttpGet("paseadores/{id}")]
        public async Task<ActionResult<Paseador>> GetPaseador(int idPaseador)
        {
            var paseador = await _context.Paseadors.FindAsync(idPaseador);

            if (paseador == null)
            {
                return NotFound();
            }

            return paseador;
        }

        [HttpPut("paseadores/{id}")]
        public async Task<IActionResult> PutPaseador(int idPaseador, Paseador paseador)
        {
            if (idPaseador != paseador.IdPaseador)
            {
                return BadRequest();
            }

            _context.Entry(paseador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaseadorExists(idPaseador))
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

        [HttpPost("registro")] // POST: api/paseadores Aqui insertamos un nuevo paseador escrito en el body en formato JSON en Postman
        public async Task<ActionResult<PaseadorDto>> Registro(RegistroPaseadorDto registroPaseadorDto)
        {
            if (await PaseadorExiste(registroPaseadorDto.Email))
            {
                return BadRequest("El paseador ya existe");
            }

            var paseador = new Paseador
            {
                DniPaseador = registroPaseadorDto.DniPaseador,
                Nombre = registroPaseadorDto.Nombre,
                Apellido = registroPaseadorDto.Apellido,
                Dirección = registroPaseadorDto.Dirección,
                Email = registroPaseadorDto.Email,
                Password = registroPaseadorDto.Password,
                TelefonoPaseador = registroPaseadorDto.TelefonoPaseador
            };

            _context.Paseadors.Add(paseador);
            await _context.SaveChangesAsync();

            return new PaseadorDto
            {
                Email = paseador.Email,
                Token = _tokenServicioPaseador.CrearTokens(paseador)
            };
        }


        private async Task<bool> PaseadorExiste(string dniPaseador)
        {
            return await _context.Paseadors.AnyAsync(x => x.DniPaseador == dniPaseador);
        }

        [HttpPost("login paseadores")]
        public async Task<ActionResult<PaseadorDto>> Login(LoginPaseadorDto loginPaseadorDto)
        {
            var paseador = await _context.Paseadors.FirstOrDefaultAsync(x => x.Email == loginPaseadorDto.Email && x.Password == loginPaseadorDto.Password);

            if (paseador == null)
            {
                return Unauthorized("Usuario no existe");
            }

            if (paseador.Password != loginPaseadorDto.Password)
            {
                return Unauthorized("Contraseña incorrecta");
            }

            return new PaseadorDto
            {
                Email = paseador.Email,
                Token = _tokenServicioPaseador.CrearTokens(paseador)
            };
        }




        [HttpDelete("paseadores/{id}")]
        public async Task<IActionResult> DeletePaseador(int idPaseador)
        {
            var paseador = await _context.Paseadors.FindAsync(idPaseador);
            if (paseador == null)
            {
                return NotFound();
            }

            _context.Paseadors.Remove(paseador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaseadorExists(int idPaseador)
        {
            return _context.Paseadors.Any(e => e.IdPaseador == idPaseador);
        }
    }
}
