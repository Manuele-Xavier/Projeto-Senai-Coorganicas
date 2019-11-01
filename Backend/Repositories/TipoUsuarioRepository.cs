using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuario
    {
        public async Task<TipoUsuario> Alterar(TipoUsuario tipousuario)
        {
            using(CoorganicasContext _contexto=new CoorganicasContext()){
                _contexto.Entry (tipousuario).State = EntityState.Modified;
                await _contexto.SaveChangesAsync ();
                return tipousuario;
            }
        }

        public async Task<TipoUsuario> BuscarPorID(int id)
        {
            using(CoorganicasContext _contexto=new CoorganicasContext()){
                return await _contexto.TipoUsuario.FindAsync(id);
            }
            
        }

        public async Task<TipoUsuario> Excluir(TipoUsuario tipoUsuario)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                _contexto.TipoUsuario.Remove (tipoUsuario);
                await _contexto.SaveChangesAsync ();
                return tipoUsuario;
            }
            
        }

        public async  Task<List<TipoUsuario>> Listar()
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                return  await _contexto.TipoUsuario.ToListAsync();
            }
        }

        public async Task<TipoUsuario> Salvar(TipoUsuario tipousuario)
        {
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync (tipousuario);
                //Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync ();
                return tipousuario;
            }
            
        }
    }
}