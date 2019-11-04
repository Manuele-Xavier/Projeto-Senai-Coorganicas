using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;

namespace Backend.Interfaces
{
    public interface IEndereco
    {
        Task<List<Endereco>> Listar();
        Task<Endereco> BuscaEndereco(int id);

        Task<Endereco> Alterar(Endereco endereco);

        Task<Endereco> Gravar(Endereco endereco); 

        Task<Endereco> Excluir(Endereco endereco); 
    }
}