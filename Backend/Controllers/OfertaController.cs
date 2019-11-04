using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Agricultor")]
    public class OfertaController : ControllerBase 
    {
          OfertaRepository _repositorio = new OfertaRepository();

        //GET: api/Oferta
        [HttpGet]
        public async Task<ActionResult<List<Oferta>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var Ofertas = await _repositorio.Listar();
            if(Ofertas == null) {
                return NotFound();
            }

            return Ofertas;
        }
        //GET: api/Oferta/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Oferta>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var Oferta = await _repositorio.BuscarPorID(id);
            if (Oferta == null) {
                return NotFound ();
            }

            return Oferta;
        }

        //POST api/Oferta
        [HttpPost]
        public async Task<ActionResult<Oferta>> Post ([FromForm]Oferta Oferta) {
            try {
                // UploadController upload =  new UploadController();
                // Oferta.Oferta.ImagemOferta = upload.Upload();
                //Tratamos contra ataques de SQL Injection
                await _repositorio.Salvar(Oferta);
              

            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return Oferta;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Oferta Oferta) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != Oferta.OfertaId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            try {
                await _repositorio.Alterar (Oferta);
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var Oferta_valido = await _repositorio.BuscarPorID(id);
                if (Oferta_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/Oferta/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Oferta>> Delete (int id) {

            var Oferta = await _repositorio.BuscarPorID(id);
            if (Oferta == null) {
                return NotFound ();
            }
           
            await _repositorio.Excluir(Oferta);

            return Oferta;
        }
    }
}