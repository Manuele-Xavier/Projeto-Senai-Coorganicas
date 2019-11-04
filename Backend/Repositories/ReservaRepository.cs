using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class ReservaRepository : IReserva
    {
        public async Task<Reserva> Alterar(Reserva reserva)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(reserva).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return reserva;                 
            }           
        }

        public async Task<Reserva> BuscarPorID(int id)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Reserva.Include("Usuario").Include("Oferta").FirstOrDefaultAsync(t => t.ReservaId == id);
            }
        }

        public async Task<Reserva> Excluir(Reserva reserva)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                 _contexto.Reserva.Remove(reserva);

                await _contexto.SaveChangesAsync();

                return reserva;
            }
        }

        public async Task<List<Reserva>> Listar()
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Reserva.Include("Usuario").Include("Oferta").ToListAsync();
            }
        }


        public async Task<Reserva> Salvar(Reserva reserva)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(reserva);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return reserva;
            }
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}