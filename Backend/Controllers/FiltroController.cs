using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class FiltroController : ControllerBase
    {
       CoorganicasContext _contexto = new CoorganicasContext();   

       [HttpGet]
       public async Task<ActionResult<List<Oferta>>> Get([FromForm]Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Cidade == oferta.Cidade && item.Regiao == oferta.Regiao && item.ProdutoId == oferta.ProdutoId) {
                    produtos.Add(item);
                }
            }

            if(ofertas == null) {
                 return NotFound(
                    new
                    {
                        Mensagem = "Produto não encontrado",
                        Erro = true
                    });     
            }

            return produtos;
       }
    }
}