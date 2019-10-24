using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var usuarios = await _contexto.Usuario.Include("TipoUsuario").ToListAsync();
            if(usuarios == null) {
                return NotFound();
            }

            return usuarios;
        }
        //GET: api/Usuario/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Usuario>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var usuario = await _contexto.Usuario.Include("TipoUsuario").FirstOrDefaultAsync(u => u.UsuarioId == id);
            if (usuario == null) {
                return NotFound ();
            }

            return usuario;
        }

        //POST api/Usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post (Usuario usuario) {
            try {
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(usuario);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return usuario;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Usuario usuario) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != usuario.UsuarioId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            _contexto.Entry (usuario).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var usuario_valido = await _contexto.Usuario.FindAsync (id);
                if (usuario_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/usuario/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Usuario>> Delete (int id) {

            var usuario = await _contexto.Usuario.FindAsync (id);
            if (usuario == null) {
                return NotFound ();
            }
            _contexto.Usuario.Remove (usuario);
            await _contexto.SaveChangesAsync ();

            return usuario;
        }
    }
}