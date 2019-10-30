using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            var Cidade = Request.Form["cidade"].Count > 0 ? Request.Form["cidade"].ToString() : null;
            var Regiao = Request.Form["regiao"].Count > 0 ? Request.Form["regiao"].ToString() : null;
            var Validade = Request.Form["validade"].Count > 0 ? Request.Form["validade"].ToString() : null;             
           
            
            if(Cidade != null && Regiao != null && Validade == null) {                
                return await CidadeRegiao(oferta);
            }
            else if(Cidade != null && Validade == null && Regiao == null) {
                return await RetornaCidade(oferta);
            }
            else if (Regiao != null && Cidade == null && Validade == null) {
                return await RetornaRegiao(oferta);
            }
            else if (Validade != null && Cidade == null && Regiao == null) {
                 return await RetornaValidade(oferta);
            }
            else if (Cidade != null && Regiao != null && Validade != null) {
                return await FiltrarTodos(oferta);
            }   
            else if(Cidade != null && Regiao == null && Validade != null) {
                return await CidadeValidade(oferta);
            }
            else if(Cidade == null && Regiao != null && Validade != null) {
                return await RegiaoValidade(oferta);
            }
            else {
                 return NotFound(
                    new
                    {
                        Mensagem = "Produto não encontrado",
                        Erro = true
                    });     
            }       
       }       
        private async Task<ActionResult<List<Oferta>>> FiltrarTodos(Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Cidade == oferta.Cidade && item.Regiao == oferta.Regiao && item.Quantidade > 0 && item.Validade == oferta.Validade) {
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
       private async Task<ActionResult<List<Oferta>>> CidadeRegiao(Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Cidade == oferta.Cidade && item.Regiao == oferta.Regiao && item.Quantidade > 0) {
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

        private async Task<ActionResult<List<Oferta>>> RegiaoValidade(Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Validade == oferta.Validade && item.Regiao == oferta.Regiao && item.Quantidade > 0) {
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
    
         private async Task<ActionResult<List<Oferta>>> CidadeValidade(Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Cidade == oferta.Cidade && item.Validade == oferta.Validade && item.Quantidade > 0) {
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

       private async Task<ActionResult<List<Oferta>>> RetornaCidade(Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Cidade == oferta.Cidade && item.Quantidade > 0) {
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

        private async Task<ActionResult<List<Oferta>>> RetornaRegiao(Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Regiao == oferta.Regiao && item.Quantidade > 0) {
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

       private async Task<ActionResult<List<Oferta>>> RetornaValidade(Oferta oferta){
            List<Oferta> produtos = new List<Oferta>();

            var ofertas =  await _contexto.Oferta.Include("Produto").ToListAsync();

            foreach(var item in ofertas) {
                
                if(item.Validade == oferta.Validade && item.Quantidade > 0) {
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