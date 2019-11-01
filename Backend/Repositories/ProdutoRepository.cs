using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class ProdutoRepository : IProduto
    {
        public async Task<Produto> Alterar(Produto produto)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(produto).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return produto;                 
            }           
        }

        public async Task<Produto> BuscarPorID(int id)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Produto.FindAsync(id);
            }
        }

        public async Task<Produto> Excluir(Produto produto)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                 _contexto.Produto.Remove(produto);

                await _contexto.SaveChangesAsync();

                return produto;
            }
        }

        public async Task<List<Produto>> Listar()
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               return await _contexto.Produto.ToListAsync();
            }
        }


        public async Task<Produto> Salvar(Produto produto)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(produto);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return produto;
            }
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}