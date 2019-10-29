using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Comunidade")]
    public class ReservaController : ControllerBase 
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/Reserva
        
        [HttpGet]
        public async Task<ActionResult<List<Reserva>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
        var reservas = await _contexto.Reserva.Include("Usuario").Include("Oferta").ToListAsync();
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
            var reserva = await _contexto.Reserva.Include("Usuario").Include("Oferta").FirstOrDefaultAsync(t => t.ReservaId == id);
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
                await _contexto.AddAsync(reserva);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

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

            _contexto.Entry (reserva).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var reserva_valido = await _contexto.Reserva.FindAsync (id);
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

            var reserva = await _contexto.Reserva.FindAsync (id);
            if (reserva == null) {
                return NotFound ();
            }
            _contexto.Reserva.Remove (reserva);
            await _contexto.SaveChangesAsync ();

            return reserva;
        }
    }
}