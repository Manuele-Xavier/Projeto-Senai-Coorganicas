using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class BuscaController : ControllerBase
    {
        CoorganicasContext _contexto = new CoorganicasContext();   

        [HttpGet]
        public async Task<ActionResult<List<Oferta>>> Get([FromForm]string produto){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();
            var p = await _contexto.Produto.FirstOrDefaultAsync(x => x.Nome == produto);

            foreach(var item in ofertas) {
                if(item.ProdutoId == p.ProdutoId) {

                    produtos.Add(item);
                }               
              
            }

            if(produtos.Count == 0) {
                return NotFound(
                new
                {
                    Mensagem = "Produto não está sendo ofertado no momento",
                    Erro = true
                });   
            }

            return produtos;
        }
    }
}