using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class OfertaRepository : IOferta
    {
        public async Task<Oferta> Alterar(Oferta oferta)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(oferta).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return oferta;                 
            }           
        }

        public async Task<Oferta> BuscarPorID(int id)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Oferta.Include("Usuario").Include("Produto").FirstOrDefaultAsync(o => o.OfertaId == id);
            }
        }

        public async Task<Oferta> Excluir(Oferta oferta)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                 _contexto.Oferta.Remove(oferta);

                await _contexto.SaveChangesAsync();

                return oferta;
            }
        }

        public async Task<List<Oferta>> Listar()
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Oferta.Include("Usuario").Include("Produto").ToListAsync();
            }
        }


        public async Task<Oferta> Salvar(Oferta oferta)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(oferta);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return oferta;
            }
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}