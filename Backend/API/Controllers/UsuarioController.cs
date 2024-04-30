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
    public class UsuarioController : BaseApiController
    {
        private readonly DogWalkPlusContext _context;
        private readonly ITokenServicio _tokenServicio;

        public UsuarioController(DogWalkPlusContext context, ITokenServicio tokenServicio)
        {
            _context = context;
            _tokenServicio = tokenServicio;
        }

        [Authorize]
        [HttpGet("usuarios")] // GET: api/usuarios Obtenemos todos los usuarios
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [Authorize]
        [HttpGet("usuarios/{id}")] // GET: api/usuarios/5 Aqui obtenemos un usuario con un id específico
        public async Task<ActionResult<Usuario>> GetUsuario(int idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPut("usuarios/{id}")] // PUT: api/usuarios/5 Actualizamos un usuario con un id específico
        public async Task<IActionResult> PutUsuario(int idUsuario, Usuario usuario)
        {
            if (idUsuario != usuario.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(idUsuario))
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

        [HttpPost("registro")] // POST: api/usuarios Aqui insertamos un nuevo usuario escrito en el body en formato JSON en Postman
        public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto registroDto)
        {
            if (await UsuarioExiste(registroDto.Email))
            {
                return BadRequest("El usuario ya existe");
            }

            var usuario = new Usuario
            {
                DniUsuario = registroDto.DniUsuario,
                Nombre = registroDto.Nombre,
                Apellido = registroDto.Apellido,
                Dirección = registroDto.Dirección,
                Email = registroDto.Email,
                Password = registroDto.Password,
                TelefonoUsuario = registroDto.TelefonoUsuario
            };
            
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDto
            {
                Email = usuario.Email,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }
      
        private async Task<bool> UsuarioExiste(string username)
        {
            return await _context.Usuarios.AnyAsync(x => x.Email == username.ToLower());
        }

        [HttpPost("login")] // POST: api/usuarios/login Aqui hacemos login con un usuario escrito en el body en formato JSON en Postman
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (usuario == null)
            {
                return Unauthorized("Usuario no existe");
            }

            if (usuario.Password != loginDto.Password)
            {
                return Unauthorized("Contraseña incorrecta");
            }

            return new UsuarioDto
            {
                Email = usuario.Email,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }



        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> DeleteUsuario(int idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int idUsuario) // Metodo para verificar si un usuario existe
        {
            return _context.Usuarios.Any(e => e.IdUsuario == idUsuario);
        }
    }
}
