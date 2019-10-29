using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Comunidade")]
    public class ReceitaController : ControllerBase 
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/Receita
        [HttpGet]
        public async Task<ActionResult<List<Receita>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var Receitas = await _contexto.Receita.Include("Usuario").ToListAsync();
            if(Receitas == null) {
                return NotFound();
            }

            return Receitas;
        }
        //GET: api/Receita/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Receita>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var Receita = await _contexto.Receita.Include("Usuario").FirstOrDefaultAsync(r => r.ReceitaId == id);
            if (Receita == null) {
                return NotFound ();
            }

            return Receita;
        }

        //POST api/Receita
        [HttpPost]
        public async Task<ActionResult<Receita>> Post ([FromForm]Receita Receita) {
            try {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine ("Imagens");
                var pathToSave = Path.Combine (Directory.GetCurrentDirectory (), folderName);

                if (file.Length > 0) {
                    var fileName = ContentDispositionHeaderValue.Parse (file.ContentDisposition).FileName.Trim ('"');
                    var fullPath = Path.Combine (pathToSave, fileName);
                    var dbPath = Path.Combine (folderName, fileName);

                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        file.CopyTo (stream);
                    }
                    
                   Receita.ImagemReceita = fileName;
                }
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(Receita);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return Receita;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Receita Receita) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != Receita.ReceitaId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            _contexto.Entry (Receita).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var Receita_valido = await _contexto.Receita.FindAsync (id);
                if (Receita_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/Receita/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Receita>> Delete (int id) {

            var Receita = await _contexto.Receita.FindAsync (id);
            if (Receita == null) {
                return NotFound ();
            }
            _contexto.Receita.Remove (Receita);
            await _contexto.SaveChangesAsync ();

            return Receita;
        }
    }
}