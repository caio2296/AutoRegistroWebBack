using Aplicacao.Interfaces;
using AutoRegistro.Token;
using AutoRegistroWebBack.Models;
using Dominio.Interfaces.InterfacesServicos;
using Dominio.Servicos;
using Entidades.Entidades;
using Entidades.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace AutoRegistro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IAplicacaoUsuario _aplicacaoUsuario;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly TokenJwtBuilder _tokenJwtBuilder;

        public UsuarioController(IAplicacaoUsuario aplicacaoUsuario, UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager, TokenJwtBuilder tokenJwtBuilder)
        {
            _aplicacaoUsuario = aplicacaoUsuario;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenJwtBuilder = tokenJwtBuilder;
        }

        //[Authorize(Policy = "RequireAdministratorRole")]
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroModel registro)
        {
            if (string.IsNullOrWhiteSpace(registro.Email) || string.IsNullOrWhiteSpace(registro.PasswordHash))
                return Ok("Falta alguns dados");


            var user = new Usuario
            {
                UserName=registro.NomeEmpresa,
                NomeEmpresa = registro.NomeEmpresa,
                Email = registro.Email,
                Celular = registro.Celular,
                Tipo = TipoUsuario.Comum
            };

            var resultado = await _userManager.CreateAsync(user, registro.PasswordHash);

            if (resultado.Errors.Any())
            {
                return BadRequest("Erro ao criar usuário");
            }

            // Geração de Confirmação caso precise
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);
            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com Sucesso");
            else
                return BadRequest("Erro ao confirmar email");
        }
        [AllowAnonymous]
        [HttpPost("/api/CriarToken")]
        [Produces("application/json")]
        public async Task<IActionResult> CriarToken([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                return Ok("Falta alguns dados");

            if (!_userManager.Users.Any())
            {
                return BadRequest("Não foi possivel encontrar o Usuario!");
            }
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return Unauthorized(" Usuario não autorizado, verifique seu email e senha!"); // O usuário não foi encontrado
            var resultado = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);
            if (!resultado.Succeeded)
                return Unauthorized("Usuario não autorizado, verifique seu email e senha!"); // Autenticação falhou

            var idUsuario = await _aplicacaoUsuario.RetornaIdUsuario(login.Email);
            var tipoUsuario = await _aplicacaoUsuario.RetornarTipoUsuario(login.Email);

            var token = _tokenJwtBuilder.GerarTokenJwt(idUsuario, tipoUsuario);

            return Ok(token.value); // Autenticação bem-sucedida, retorne o token
        }

        [Authorize] // Garante que apenas usuários autenticados acessam
        [HttpGet("/api/ObterUsuario")]
        public async Task<IActionResult> ObterUsuario()
        {
            var userId = User.FindFirst("idUsuario")?.Value;


            if (userId == null)
                return Unauthorized("Usuário não autenticado");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("Usuário não encontrado");

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.Celular,
                user.NomeEmpresa,
                user.Tipo
            });
        }

        [Authorize] // Garante que o usuário esteja autenticado
        [HttpPut("api/AlterarSenha")]
        [Produces("application/json")]
        public async Task<ActionResult> AlterarSenha([FromBody] AlterarSenhaModel alterarSenhaModel)
        {
            // Verifica se o modelo de dados foi enviado corretamente
            if (alterarSenhaModel == null)
            {
                return BadRequest("Dados de alteração de senha não podem ser nulos.");
            }

            // Verifica se as senhas foram fornecidas
            if (string.IsNullOrWhiteSpace(alterarSenhaModel.SenhaAtual) || string.IsNullOrWhiteSpace(alterarSenhaModel.NovaSenha))
            {
                return BadRequest("A senha atual e a nova senha são obrigatórias.");
            }

            // Obtém o usuário autenticado
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Verifica se a senha atual fornecida é válida
            var resultadoSenhaAtual = await _userManager.CheckPasswordAsync(usuario, alterarSenhaModel.SenhaAtual);
            if (!resultadoSenhaAtual)
            {
                return Unauthorized("Senha atual incorreta.");
            }

            // Altera a senha
            var resultadoAlteracaoSenha = await _userManager.ChangePasswordAsync(usuario, alterarSenhaModel.SenhaAtual, alterarSenhaModel.NovaSenha);
            if (!resultadoAlteracaoSenha.Succeeded)
            {
                // Se ocorrer algum erro, retorna um BadRequest com os erros
                return BadRequest(resultadoAlteracaoSenha.Errors);
            }

            // Retorna uma resposta de sucesso
            return Ok("Senha alterada com sucesso.");
        }

        //[AllowAnonymous]
        //[HttpPost("/api/RegistrarUsuarioFoto")]

        //public async Task<IActionResult> RegistrarUsuarioFoto([FromForm] Registro registro)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(registro.email) || string.IsNullOrWhiteSpace(registro.senha))
        //            return Ok("Falta alguns dados");
        //        string nomeFoto = "";

        //        if (registro != null)
        //        {
        //            // Processar o arquivo aqui (por exemplo, salvar no disco, banco de dados, etc.)
        //            var filePath =
        //            Path.Combine(RetornarCaminhoFoto(), registro.foto.FileName);

        //            nomeFoto = registro.foto.FileName;
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await registro.foto.CopyToAsync(stream);
        //            }
        //        }


        //        var caminhoArquivo = Path.Combine(RetornarCaminhoFoto(), nomeFoto);

        //        var user = new Usuario
        //        {
        //            UserName = registro.email,
        //            Email = registro.email,
        //            Celular = registro.celular,
        //            Tipo = TipoUsuario.Comum,
        //            UrlFoto = caminhoArquivo
        //        };

        //        var resultado = await _userManager.CreateAsync(user, registro.senha);

        //        if (resultado.Errors.Any())
        //        {
        //            return Ok(resultado.Errors);
        //        }

        //        // Geração de Confirmação caso precise
        //        var userId = await _userManager.GetUserIdAsync(user);
        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        //        //retorno email
        //        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        //        var resultado2 = await _userManager.ConfirmEmailAsync(user, code);
        //        if (resultado2.Succeeded)
        //            return Ok("Usuário Adicionado com Sucesso");
        //        else
        //            return Ok("Erro ao confirmar usuários");

        //        // Retorne uma resposta de sucesso ou qualquer outra resposta necessária.
        //        return Ok("Upload de arquivo(s) bem-sucedido!");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Trate erros aqui e retorne uma resposta de erro apropriada.
        //        return StatusCode(500, "Erro no servidor: " + ex.Message);
        //    }
        //}

        //private static string RetornarCaminhoFotoPerfil()
        //{
        //    string diretorioAtual = Directory.GetCurrentDirectory();
        //    string diretorioProfile = Directory.GetParent(diretorioAtual).FullName;
        //    string diretorioInfra = Path.Combine(diretorioProfile, "Infraestrutura");
        //    string diretorioRepositorio = Path.Combine(diretorioInfra, "Repositorio");
        //    string diretorioImagem = Path.Combine(diretorioRepositorio, "ImagemPerfil");
        //    string nomeFoto = "PerfilCaio.jpg";

        //    string caminhoFoto = Path.Combine(diretorioImagem, nomeFoto);
        //    return caminhoFoto;
        //}

        //private static string RetornarCaminhoFoto()
        //{
        //    string diretorioAtual = Directory.GetCurrentDirectory();
        //    string diretorioProfile = Directory.GetParent(diretorioAtual).FullName;
        //    string diretorioInfra = Path.Combine(diretorioProfile, "Infraestrutura");
        //    string diretorioRepositorio = Path.Combine(diretorioInfra, "Repositorio");
        //    string diretorioImagem = Path.Combine(diretorioRepositorio, "ImagemPerfil");

        //    return diretorioImagem;
        //}
    }

}