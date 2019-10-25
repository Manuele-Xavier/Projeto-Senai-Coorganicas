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

           var Enderecos = await _contexto.Endereco.Include("Usuario").ToListAsync();
            if (Enderecos == null) {
                return NotFound();
            }

            return Enderecos;
        }
        //GET: api/Endereco2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Endereco>> Get (int id) {
            
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
               
                await _contexto.AddAsync (Endereco);
                
                await _contexto.SaveChangesAsync ();

            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Endereco;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Endereco Endereco) {

            if (id != Endereco.EnderecoId) {
                return BadRequest ();
            }

            _contexto.Entry (Endereco).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                var Endereco_valido = await _contexto.Endereco.FindAsync (id);
                if (Endereco_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            
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