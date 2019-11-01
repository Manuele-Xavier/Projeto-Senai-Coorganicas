using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class ReceitaRepository : IReceita
    {
        public async Task<Receita> Alterar(Receita receita)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(receita).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return receita;                 
            }           
        }

        public async Task<Receita> BuscarPorID(int id)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Receita.Include("Usuario").FirstOrDefaultAsync(r => r.ReceitaId == id);;
            }
        }

        public async Task<Receita> Excluir(Receita receita)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                 _contexto.Receita.Remove(receita);

                await _contexto.SaveChangesAsync();

                return receita;
            }
        }

        public async Task<List<Receita>> Listar()
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Receita.Include("Usuario").ToListAsync();
            }
        }


        public async Task<Receita> Salvar(Receita receita)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(receita);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return receita;
            }
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}