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
    [Authorize(Roles = "Administrador")]
    public class ProdutoController : ControllerBase
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/Produto
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var produtos = await _contexto.Produto.ToListAsync();
            if(produtos == null) {
                return NotFound();
            }

            return produtos;
        }
        //GET: api/Produto/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Produto>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var produto = await _contexto.Produto.FindAsync (id);
            if (produto == null) {
                return NotFound ();
            }

            return produto;
        }

        //POST api/Produto
        [HttpPost]
        public async Task<ActionResult<Produto>> Post ([FromForm]Produto produto) {
            try {
                 if (Request.Form.Files.Count > 0) {
                    
                    var file = Request.Form.Files[0];
                    var folderName = Path.Combine ("Imagens");
                    var pathToSave = Path.Combine (Directory.GetCurrentDirectory (), folderName);
                    var fileName = ContentDispositionHeaderValue.Parse (file.ContentDisposition).FileName.Trim ('"');
                    var fullPath = Path.Combine (pathToSave, fileName);
                    var dbPath = Path.Combine (folderName, fileName);

                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        file.CopyTo (stream);
                    }                    

                    
                   produto.ImagemProduto = fileName;
                   produto.Nome = Request.Form["Nome"];
                   produto.Descricao = Request.Form["Descricao"];                
                  

                } else {
                   return NotFound(
                    new
                    {
                        Mensagem = "Atenção a imagem não foi selecionada!",
                        Erro = true
                    });        
                }          

                
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(produto);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return produto;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Produto produto){

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != produto.ProdutoId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            _contexto.Entry(produto).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var produto_valido = await _contexto.Produto.FindAsync(id);
                if (produto_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/produto/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Produto>> Delete (int id) {

            var produto = await _contexto.Produto.FindAsync (id);
            if (produto == null) {
                return NotFound ();
            }
            _contexto.Produto.Remove(produto);
            await _contexto.SaveChangesAsync ();

            return produto;
        }
    }
}