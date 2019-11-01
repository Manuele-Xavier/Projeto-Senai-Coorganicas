using System.Linq;
using Backend.Domains;
using Backend.Interfaces;
using Backend.ViewModels;

namespace Backend.Repositories
{
    public class LoginRepository : ILogin
    {
        public Usuario Logar(LoginViewModel login)
        {   
            using(CoorganicasContext _contexto = new CoorganicasContext()){
                var usuario = _contexto.Usuario.FirstOrDefault(
                    u => u.Email == login.Email && u.Senha == login.Senha
                );

                return usuario;
            }
        }
    }
}