using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase 
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/Endereco
        [HttpGet]
        public async Task<ActionResult<List<Endereco>>> Get() {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
           var Enderecos = await _contexto.Endereco.Include("Usuario").ToListAsync();
            if (Enderecos == null) {
                return NotFound();
            }

            return Enderecos;
        }
        //GET: api/Endereco2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Endereco>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var Endereco = await _contexto.Endereco.Include("Usuario").FirstOrDefaultAsync(e => e.EnderecoId == id);
            if (Endereco == null) {
                return NotFound ();
            }

            return Endereco;
        }

        //POST api/Endereco
        [HttpPost]
        public async Task<ActionResult<Endereco>> Post (Endereco Endereco) {
            try {
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync (Endereco);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync ();

            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Endereco;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Endereco Endereco) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != Endereco.EnderecoId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            _contexto.Entry (Endereco).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var Endereco_valido = await _contexto.Endereco.FindAsync (id);
                if (Endereco_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/Endereco/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Endereco>> Delete (int id) {

            var Endereco = await _contexto.Endereco.FindAsync (id);
            if (Endereco == null) {
                return NotFound ();
            }
            _contexto.Endereco.Remove (Endereco);
            await _contexto.SaveChangesAsync ();

            return Endereco;
        }
    }
}