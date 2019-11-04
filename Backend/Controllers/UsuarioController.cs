using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net.Http.Headers;
using Backend.Domains;
using Backend.Repositories;
using Backend.ViewModels;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class UsuarioController : ControllerBase
    {
        UsuarioRepository _repositorio = new UsuarioRepository();
        //GET: api/Usuario
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get(){
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var usuarios = await _repositorio.Listar();

            if(usuarios == null) {
                return NotFound();
            }

            return usuarios;
        }
        //GET: api/Usuario/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Usuario>> Get(int id) {
            //FindAsync = procurar algo especifico no banco
            //await espera acontecer 
            var usuario = await _repositorio.BuscarPorID(id);
            if (usuario == null) {
                return NotFound ();
            }

            return usuario;
        }

        //POST api/Usuario
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post([FromForm]CadastrarUsuarioViewModel usuario) {
            try {       
                Usuario NovoUsuario = new Usuario();

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

                   NovoUsuario.ImagemUsuario = fileName;
                   NovoUsuario.Nome = usuario.Nome;  
                   NovoUsuario.Email = usuario.Email;
                   NovoUsuario.Cnpj = usuario.Cnpj;
                   NovoUsuario.Senha = usuario.Senha;
                   NovoUsuario.TipoUsuarioId = Convert.ToInt32(usuario.TipoUsuarioId);

                } else {
                    var fileName = string.Empty;

                    if(Request.Form.Files.Count == 0) {                    
                        if(Convert.ToInt32(Request.Form["TipoUsuario"]) == 2) {
                            fileName = "woman.png";
                        } else {
                            
                            fileName = "pessoa.png";
                        }                   
                    }    

                   NovoUsuario.ImagemUsuario = fileName;
                   NovoUsuario.Nome = usuario.Nome;  
                   NovoUsuario.Email = usuario.Email;
                   NovoUsuario.Cnpj = usuario.Cnpj;
                   NovoUsuario.Senha = usuario.Senha;
                   NovoUsuario.TipoUsuarioId = Convert.ToInt32(usuario.TipoUsuarioId);
                }
                
                if(ValidaCNPJ(usuario.Cnpj) == true){                    
                    await _repositorio.Gravar(NovoUsuario);

                    // Após gravar o usuário no banco iremos gravar os dados de enderco e telefone
                    // Aqui iremos chamar um metodo que retorna o ultimo usuario cadastrado que no caso foi o que acabamos de gravar
                    var UsuarioGravado = await _repositorio.RetornarUltimoUsuarioCadastrado();    
                    GravarTelEnd(UsuarioGravado.UsuarioId, usuario);

                    return await _repositorio.BuscarPorID(UsuarioGravado.UsuarioId);
                }
                else{
                    return BadRequest();
                }

            } catch (DbUpdateConcurrencyException) {
                throw;
            }          
        }

        
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id,[FromForm] UsuarioViewModel usuario) {
           // Verifica se existe o usuario no banco através do id passado por parametro
            var ExisteUsuario = await _repositorio.BuscarPorID(id);
            
            TelefoneRepository _tel = new TelefoneRepository();
            var Tel = await _tel.BuscaTelefone(id) != null ? await _tel.BuscaTelefone(id) : null; 

            EnderecoRepository _end = new EnderecoRepository();
            var End = await _end.BuscaEndereco(id) != null ? await _end.BuscaEndereco(id) : null;

            //Se o Id do objeto não existir
            if(Tel == null) {
                return NotFound(
                new
                {
                    Mensagem = "Usuário não encontrado.",
                    Erro = true
                });   
            }  
                        
            var UsuarioAlterado = VerificaAltercao(ExisteUsuario, usuario);
            var TelefoneAlterado = VerificaAltercaoTel(Tel, usuario);
            var EnderecoAlterado = VerificaAltercaoEndereco(End, usuario);

            try {
                var user = await _repositorio.Alterar(UsuarioAlterado);
                var tel = await _tel.Alterar(TelefoneAlterado);
                var end = await _end.Alterar(EnderecoAlterado);
               
            } catch (DbUpdateConcurrencyException) {
                //Verificamos se o objeto realmente existe no banco
                var usuario_valido = await _repositorio.BuscarPorID(id);
                
                if (usuario_valido == null) {
                    return NotFound ();
                } else {
                    throw;
                }

            }
            // NoContent = retorna o erro 204, sem nada
            return Ok(
                new
                {
                    Mensagem = "Usuário alterado com sucesso.",
                    Erro = false
                }
            );   
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Usuario>> Delete (int id) {

            var usuario = await _repositorio.BuscarPorID(id);
            
            if (usuario == null) {
                return NotFound ();
            }
            
            await _repositorio.Excluir(usuario);

            return usuario;
        }

        private static bool ValidaCNPJ(string cnpjUsuario){

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
       
       private Usuario VerificaAltercao(Usuario usuario, UsuarioViewModel user) {
            // Iremos verificar se tem alguma alteração dos dados através do viewmodel caso tenha iremos atribuir
            // os valores, com a viewmodel conseguimos fazer as alterações em precisar preencher os campos obrigatórios
                        
            if(usuario.Nome != user.Nome && user.Nome != null) {
                usuario.Nome = user.Nome;
            }

            if(usuario.Cnpj != user.Cnpj && user.Cnpj != null) {
                usuario.Cnpj = user.Cnpj;
            }

            if(usuario.Senha != user.Senha && user.Senha != null ) {
                usuario.Senha = user.Senha;
            }

            if(usuario.Email != user.Email && user.Email != null) {
                usuario.Email = user.Email;
            }
            
            if(usuario.TipoUsuarioId != Convert.ToInt32(user.TipoUsuarioId)) {
                if(Convert.ToInt32(user.TipoUsuarioId) > 0 && (Convert.ToInt32(user.TipoUsuarioId) == 2 || Convert.ToInt32(user.TipoUsuarioId) == 3)) {
                    usuario.TipoUsuarioId = Convert.ToInt32(user.TipoUsuarioId);
                }
            }
            // usuario.TipoUsuarioId = 2;

            if(Request.Form.Files.Count > 0) {
                string caminho = "";
                string pasta = "";
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
               
            }
            
           return usuario;
       }

        private Telefone VerificaAltercaoTel(Telefone telefone, UsuarioViewModel user) {
                        
            if(telefone.Telefone1 != user.Telefone && user.Telefone != null && telefone != null) {
                telefone.Telefone1 = user.Telefone;
            }

            return telefone;
        }
        private Endereco VerificaAltercaoEndereco(Endereco endereco, UsuarioViewModel user) {
                        
            if(endereco.Cidade != user.Cidade && user.Cidade != null && endereco != null) {
                endereco.Cidade = user.Cidade;
            }

            if(endereco.Cep != user.Cep && user.Cep != null && endereco != null) {
                endereco.Cep = user.Cep;
            }

            if(endereco.Endereco1 != user.Endereco && user.Endereco != null && endereco != null) {
                endereco.Endereco1 = user.Endereco;
            }

            if(endereco.Numero != Convert.ToInt32(user.Numero)) {
                if(Convert.ToInt32(user.Numero) > 0) {
                   endereco.Numero = Convert.ToInt32(user.Numero);
                }
            }

            return endereco;
        }

        private async void GravarTelEnd(int Id, CadastrarUsuarioViewModel novousuario) {
            TelefoneRepository _telefone = new TelefoneRepository();
            Telefone NovoTelefone = new Telefone();
            NovoTelefone.Telefone1 = novousuario.Telefone;
            NovoTelefone.UsuarioId = Id;
            await _telefone.Gravar(NovoTelefone);

            EnderecoRepository _endereco = new EnderecoRepository();
            Endereco NovoEndereco = new Endereco();
            NovoEndereco.Cidade = novousuario.Cidade;
            NovoEndereco.Cep = novousuario.Cep;
            NovoEndereco.Endereco1 = novousuario.Endereco;
            NovoEndereco.Numero = novousuario.Numero;
            NovoEndereco.UsuarioId = Id;
            await _endereco.Gravar(NovoEndereco);
        }
            
    }
}