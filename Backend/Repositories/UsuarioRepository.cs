using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        public async Task<Usuario> Alterar(Usuario usuario)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas               
                _contexto.Entry(usuario).State = EntityState.Modified;

                await _contexto.SaveChangesAsync();             
                 
                return usuario;
            }           
        }

        public async Task<Usuario> BuscarPorID(int id)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
                // Include("") = Adiciona a arvore 
               return await _contexto.Usuario.Include("TipoUsuario").Include("Telefone").Include("Endereco").FirstOrDefaultAsync(e => e.UsuarioId == id);
            }
        }

        public async Task<Usuario> Excluir(Usuario usuario)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                _contexto.Usuario.Remove (usuario);
                await _contexto.SaveChangesAsync ();

                return usuario;
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
               // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
               // Include("") = Adiciona a arvore 
               return await _contexto.Usuario.Include("TipoUsuario").Include("Telefone").Include("Endereco").ToListAsync();
              
            }
        }

        public async Task<Usuario> Gravar(Usuario usuario)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(usuario);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return usuario;
            }
        }

        public async Task<Usuario> RetornarUltimoUsuarioCadastrado() {
            using(CoorganicasContext _contexto = new CoorganicasContext()){

                return await _contexto.Usuario.OrderByDescending(x => x.UsuarioId).FirstOrDefaultAsync();
            }
        }
                   
    }
}