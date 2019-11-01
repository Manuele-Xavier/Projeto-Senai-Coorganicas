using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase 
    {
        TipoUsuarioRepository _repositorio=new TipoUsuarioRepository();
        
        //GET: api/TipoUsuario
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get() {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var tipoUsuario = await _repositorio.Listar();
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
            var tipoUsuario = await _repositorio.BuscarPorID(id);
            if (tipoUsuario == null) {
                return NotFound ();
            }

            return tipoUsuario;
        }

        //POST api/TipoUsuario

        [HttpPost]
        public async Task<ActionResult<TipoUsuario>> Post (TipoUsuario tipoUsuario) {
            try {
                await _repositorio.Salvar(tipoUsuario);

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

            

            try {
                await _repositorio.Alterar(tipoUsuario);
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var tipoUsuario_valido = await _repositorio.BuscarPorID(id);
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

            var tipoUsuario = await _repositorio.BuscarPorID(id);
            if (tipoUsuario == null) {
                return NotFound ();
            }
            await _repositorio.Excluir(tipoUsuario);
            return tipoUsuario;
        }
    }
}