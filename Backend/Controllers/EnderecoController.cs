using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase 
    {
       EnderecoRepository _repositorio = new EnderecoRepository();

        //GET: api/Endereco
        [HttpGet]
        public async Task<ActionResult<List<Endereco>>> Get() {

           var Enderecos = await _repositorio.Listar();
            if (Enderecos == null) {
                return NotFound();
            }

            return Enderecos;
        }
        //GET: api/Endereco2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Endereco>> Get (int id) {
            
            var Endereco = await _repositorio.BuscarPorID(id);
            if (Endereco == null) {
                return NotFound ();
            }

            return Endereco;
        }

        //POST api/Endereco
        [HttpPost]
        public async Task<ActionResult<Endereco>> Post (Endereco Endereco) {
            try {
               
                await _repositorio.Salvar(Endereco);
                

            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Endereco;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Endereco endereco) {

            if (id != endereco.EnderecoId) {
                return BadRequest ();
            }

            try {
                await _repositorio.Alterar(endereco);
            } catch (DbUpdateConcurrencyException) {

                var Endereco_valido = await _repositorio.BuscarPorID (id);
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

            var endereco = await _repositorio.BuscarPorID (id);
            if (endereco == null) {
                return NotFound ();
            }
            await _repositorio.Excluir (endereco);

            return endereco;
        }
    }
}