using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TelefoneController : ControllerBase 
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/Telefone
        [HttpGet]
        public async Task<ActionResult<List<Telefone>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var telefones = await _contexto.Telefone.Include("Usuario").ToListAsync();
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
            var telefone = await _contexto.Telefone.Include("Usuario").FirstOrDefaultAsync(t => t.TelefoneId == id);
            if (telefone == null) {
                return NotFound ();
            }

            return telefone;
        }

        //POST api/Telefone
        [HttpPost]
        public async Task<ActionResult<Telefone>> Post (Telefone telefone) {
            try {
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(telefone);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

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

            //Comparamos os atributos que foram modificados atraves do EF

            _contexto.Entry (telefone).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var telefone_valido = await _contexto.Telefone.FindAsync (id);
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

            var telefone = await _contexto.Telefone.FindAsync (id);
            if (telefone == null) {
                return NotFound ();
            }
            _contexto.Telefone.Remove (telefone);
            await _contexto.SaveChangesAsync ();

            return telefone;
        }
    }
}