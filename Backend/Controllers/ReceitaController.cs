using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Repositories;
using Backend_Cooganicas.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class ReceitaController : ControllerBase 
    {
        ReceitaRepository _repositorio = new ReceitaRepository();

        //GET: api/Receita
        [HttpGet]
        public async Task<ActionResult<List<Receita>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var Receitas = await _repositorio.Listar();
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
            var Receita = await _repositorio.BuscarPorID(id);
            if (Receita == null) {
                return NotFound ();
            }

            return Receita;
        }

        //POST api/Receita
        [HttpPost]
        public async Task<ActionResult<Receita>> Post ([FromForm]Receita receita) {
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

                   receita.ImagemReceita = fileName;
                   receita.Titulo = Request.Form["Titulo"];
                   receita.Conteudo = Request.Form["Conteudo"];
                   receita.UsuarioId = LoginController.UsuarioLogado;
                  

                } else {
                   return NotFound(
                    new
                    {
                        Mensagem = "Atenção a imagem não foi selecionada!",
                        Erro = true
                    });        
                }          
                
                //Tratamos contra ataques de SQL Injection
                await _repositorio.Salvar(receita);
                
            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return receita;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Receita receita) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != receita.ReceitaId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            try {
                await _repositorio.Alterar(receita);
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var receita_valido = await _repositorio.BuscarPorID(id);
                if (receita_valido == null) {
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

            var receita = await _repositorio.BuscarPorID (id);
            if (receita == null) {
                return NotFound ();
            }
            
            await _repositorio.Excluir(receita);

            return receita;
        }
    }
}