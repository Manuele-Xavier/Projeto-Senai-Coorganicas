using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class TelefoneRepository
    {   
           
        public async Task<Telefone> BuscaTelefone(int id)
        {   
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
                // Include("") = Adiciona a arvore 
                var telefone = await _contexto.Telefone.Include("Usuario").FirstOrDefaultAsync(t => t.UsuarioId == id);

                return telefone;
            }
           
        }

        public async Task<Telefone> Alterar(Telefone telefone)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas               
                _contexto.Entry(telefone).State = EntityState.Modified;

                await _contexto.SaveChangesAsync();             
                 
                return telefone;
            }           
        }

        public async Task<Telefone> Gravar(Telefone telefone)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(telefone);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return telefone;
            }
        }
        public async Task<Telefone> Excluir(Telefone telefone){
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                _contexto.Telefone.Remove (telefone);
                await _contexto.SaveChangesAsync ();
                return telefone;
            }
        }

        

    }
}