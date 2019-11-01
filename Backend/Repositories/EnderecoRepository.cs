using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class EnderecoRepository : IEndereco
    {
        public async Task<Endereco> Alterar(Endereco endereco)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(endereco).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return endereco;                 
            }           
        }

        public async Task<Endereco> BuscarPorID(int id)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Endereco.Include("Usuario").FirstOrDefaultAsync(r => r.EnderecoId == id);;
            }
        }

        public async Task<Endereco> Excluir(Endereco endereco)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                 _contexto.Endereco.Remove(endereco);

                await _contexto.SaveChangesAsync();

                return endereco;
            }
        }

        public async Task<List<Endereco>> Listar()
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Endereco.Include("Usuario").ToListAsync();
            }
        }


        public async Task<Endereco> Salvar(Endereco endereco)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(endereco);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return endereco;
            }
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}