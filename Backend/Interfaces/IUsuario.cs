using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.ViewModels;

namespace Backend.Interfaces
{
    public interface IUsuario
    {
        Task<List<Usuario>> Listar();

        Task<Usuario> BuscarPorID(int id);

        Task<Usuario> Gravar(Usuario usuario); 

        Task<Usuario> Alterar(Usuario usuario);

        Task<Usuario> Excluir(Usuario usuario);
    }
}