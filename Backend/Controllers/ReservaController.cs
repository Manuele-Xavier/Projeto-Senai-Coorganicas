using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Comunidade")]
    public class ReservaController : ControllerBase 
    {
         ReservaRepository _repositorio = new ReservaRepository();

        //GET: api/Reserva
        
        [HttpGet]
        public async Task<ActionResult<List<Reserva>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
        var reservas = await _repositorio.Listar();
            if(reservas == null) {
                return NotFound();
            }

            return reservas;
        }
        //GET: api/Reserva/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Reserva>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var reserva = await _repositorio.BuscarPorID(id);
            if (reserva == null) {
                return NotFound ();
            }

            return reserva;
        }

        //POST api/Reserva
        [HttpPost]
        public async Task<ActionResult<Reserva>> Post (Reserva reserva) {
            try {
                //Tratamos contra ataques de SQL Injection
               
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _repositorio.Salvar(reserva);

            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return reserva;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Reserva reserva) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != reserva.ReservaId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

        

            try {
                await _repositorio.Alterar(reserva);
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var reserva_valido = await _repositorio.BuscarPorID(id);
                if (reserva_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        //DELETE api/reserva/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Reserva>> Delete (int id) {

            var reserva = await _repositorio.BuscarPorID (id);
            if (reserva == null) {
                return NotFound ();
            }
            
            await _repositorio.Excluir(reserva);

            return reserva;
        }
    }
}