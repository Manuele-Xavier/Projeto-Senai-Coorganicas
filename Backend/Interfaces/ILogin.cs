using Backend.Domains;
using Backend.ViewModels;

namespace Backend.Interfaces
{
    public interface ILogin
    {
         Usuario Logar(LoginViewModel login);
    }
}