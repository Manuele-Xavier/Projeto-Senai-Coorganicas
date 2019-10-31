using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class UsuarioController : ControllerBase
    {
        CoorganicasContext _contexto = new CoorganicasContext();

        //GET: api/Usuario
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var usuarios = await _contexto.Usuario.Include("Telefone").Include("Endereco").ToListAsync();
            if(usuarios == null) {
                return NotFound();
            }

            return usuarios;
        }
        //GET: api/Usuario/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Usuario>> Get (int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var usuario = await _contexto.Usuario.Include("Telefone").Include("Endereco").FirstOrDefaultAsync(u => u.UsuarioId == id);
            if (usuario == null) {
                return NotFound ();
            }

            return usuario;
        }

        //POST api/Usuario
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post ([FromForm]Usuario usuario) {
            try {       
                
                if (Request.Form.Files.Count > 0) {
                    
                    var file = Request.Form.Files[0];
                    var folderName = Path.Combine ("Imagens");
                    var pathToSave = Path.Combine (Directory.GetCurrentDirectory (), folderName);
                    var fileName = ContentDispositionHeaderValue.Parse (file.ContentDisposition).FileName.Trim ('"');
                    var fullPath = Path.Combine (pathToSave, fileName);
                    var dbPath = Path.Combine (folderName, fileName);

                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        file.CopyTo (stream);
                    }                    

                    
                   usuario.ImagemUsuario = fileName;
                   usuario.Nome = Request.Form["Nome"];
                   usuario.Email = Request.Form["Email"];
                   usuario.Cnpj =  Request.Form["Cnpj"];
                   usuario.Senha =  Request.Form["Senha"];
                   usuario.TipoUsuarioId = Convert.ToInt32(Request.Form["TipoUsuario"]);

                } else {
                    var fileName = string.Empty;

                    if(Request.Form.Files.Count == 0) {                    
                        if(Convert.ToInt32(Request.Form["TipoUsuario"]) == 2) {
                            fileName = "woman.png";
                        } else {
                            
                            fileName = "pessoa.png";
                        }                   
                    }    

                   usuario.ImagemUsuario = fileName; 
                   usuario.Nome = Request.Form["Nome"];
                   usuario.Email = Request.Form["Email"];
                   usuario.Cnpj = Request.Form["Cnpj"];
                   usuario.Senha = Request.Form["Senha"];
                   usuario.TipoUsuarioId = Convert.ToInt32(Request.Form["TipoUsuario"]);
                }
                

                //Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(usuario);
                if(ValidaCNPJ(usuario.Cnpj)==true){
                    //Salvamos efetivamente o nosso objeto no banco de dados
                    await _contexto.SaveChangesAsync();
                }
                else{
                    return BadRequest();
                }
                

            } catch (DbUpdateConcurrencyException) {
                throw;
            }

            return usuario;
        }

        
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Usuario usuario) {

            //Se o Id do objeto não existir 
            //ele retorna o erro 400

            if (id != usuario.UsuarioId) {
                return BadRequest ();
            }

            //Comparamos os atributos que foram modificados atraves do EF

            _contexto.Entry (usuario).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var usuario_valido = await _contexto.Usuario.FindAsync (id);
                if (usuario_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return NoContent ();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Usuario>> Delete (int id) {

            var usuario = await _contexto.Usuario.FindAsync (id);
            if (usuario == null) {
                return NotFound ();
            }
            _contexto.Usuario.Remove (usuario);
            await _contexto.SaveChangesAsync ();

            return usuario;
        }
        static bool ValidaCNPJ(string cnpjUsuario){

            bool resultado = false;
            
            int[] v1 = {5,4,3,2,9,8,7,6,5,4,3,2};

            string cnpjCalculo = "";
            string digito_v1 = "";
            string digito_v2 = "";

            int resto = 0;
            int calculo = 0;

            cnpjCalculo = cnpjUsuario.Substring(0,12);

            for(int i=0; i<= 11; i++){
                calculo += int.Parse(cnpjCalculo[i].ToString()) * v1[i];
            }

            resto = calculo % 11;
            calculo = 11 - resto;

            if(calculo < 2 ){
                digito_v1 = "0";
            }else{
                digito_v1 = calculo.ToString();
            }
            if(digito_v1 == cnpjUsuario[12].ToString()){
                resultado = true;
            }

            int[] v2 = {6,5,4,3,2,9,8,7,6,5,4,3,2};
            resto = 0;
            cnpjCalculo = cnpjCalculo + calculo.ToString();
            calculo = 0;

            for(int i=0; i <=12; i++){
                calculo += int.Parse(cnpjCalculo[i].ToString())*v2[i];
            }

            resto = calculo % 11;
            calculo = 11 - resto;

            if(calculo < 2 ){
                digito_v2 = "0";
            }else{
                digito_v2 = calculo.ToString();
            }
            if(digito_v2 == cnpjUsuario[13].ToString()){
                resultado = true;
            }

            return resultado;

        }
    }
}