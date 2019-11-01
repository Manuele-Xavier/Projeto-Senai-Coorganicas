using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class TelefoneRepository
    {
        public async Task<List<Telefone>> Listar(){
            using( CoorganicasContext _contexto = new CoorganicasContext()){
                return await _contexto.Telefone.Include("Usuario").ToListAsync();
            }
        }
        public async Task<Telefone> BuscarPorId(int id){
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                return await _contexto.Telefone.Include("Usuario").FirstOrDefaultAsync(t => t.TelefoneId == id);
            }
            
        }

        public async Task<Telefone> Salvar(Telefone telefone){
            using( CoorganicasContext _contexto = new CoorganicasContext()){
                 //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(telefone);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();;
                return telefone;
            }
        }

        public async Task<Telefone> Alterar(Telefone telefone){
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                //Comparamos os atributos que foram modificados atraves do EF
                _contexto.Entry (telefone).State = EntityState.Modified;
                await _contexto.SaveChangesAsync ();
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