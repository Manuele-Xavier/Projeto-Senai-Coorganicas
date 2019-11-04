using System.Threading.Tasks;
using Backend.Domains;

namespace Backend.Interfaces
{
    public interface ITelefone
    {
        Task<Telefone> BuscaTelefone(int id);

        Task<Telefone> Alterar(Telefone telefone);

        Task<Telefone> Gravar(Telefone telefone); 

        Task<Telefone> Excluir(Telefone telefone); 
    }
}