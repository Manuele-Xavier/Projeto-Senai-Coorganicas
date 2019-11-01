using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TelefoneController : ControllerBase 
    {
       TelefoneRepository _repositorio=new TelefoneRepository();

        //GET: api/Telefone
        [HttpGet]
        public async Task<ActionResult<List<Telefone>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var telefones = await _repositorio.Listar();
            if(telefones == null) { 
                return NotFound();
            }

            return telefones;
        }
        //GET: api/Telefone/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Telefone>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var telefone = await _repositorio.BuscarPorId(id);
            if (telefone == null) {
                return NotFound ();
            }

            return telefone;
        }

        //POST api/Telefone
        [HttpPost]
        public async Task<ActionResult<Telefone>> Post (Telefone telefone) {
            try {
                await _repositorio.Salvar(telefone);

            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return telefone;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Telefone telefone) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != telefone.TelefoneId) {
                return BadRequest ();
            }


            try {
                await _repositorio.Alterar(telefone);
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var telefone_valido = await _repositorio.BuscarPorId(id);
                if (telefone_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/telefone/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Telefone>> Delete (int id) {

            var telefone = await _repositorio.BuscarPorId(id);
            if (telefone == null) {
                return NotFound ();
            }
            await _repositorio.Excluir(telefone);
            return telefone;
        }
    }
}