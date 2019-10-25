using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase 
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/TipoUsuario
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get() {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var tipoUsuario = await _contexto.TipoUsuario.ToListAsync();
            if (tipoUsuario == null) {
                return NotFound();
            }

            return tipoUsuario;
        }
        //GET: api/TipoUsuario2
        [HttpGet ("{id}")]
        public async Task<ActionResult<TipoUsuario>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var tipoUsuario = await _contexto.TipoUsuario.FindAsync (id);
            if (tipoUsuario == null) {
                return NotFound ();
            }

            return tipoUsuario;
        }

        //POST api/TipoUsuario

        [HttpPost]
        public async Task<ActionResult<TipoUsuario>> Post (TipoUsuario tipoUsuario) {
            try {
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync (tipoUsuario);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync ();

            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return tipoUsuario;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, TipoUsuario tipoUsuario) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != tipoUsuario.TipoUsuarioId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            _contexto.Entry (tipoUsuario).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var tipoUsuario_valido = await _contexto.TipoUsuario.FindAsync (id);
                if (tipoUsuario_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/tipoUsuario/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<TipoUsuario>> Delete (int id) {

            var tipoUsuario = await _contexto.TipoUsuario.FindAsync (id);
            if (tipoUsuario == null) {
                return NotFound ();
            }
            _contexto.TipoUsuario.Remove (tipoUsuario);
            await _contexto.SaveChangesAsync ();

            return tipoUsuario;
        }
    }
}