using Dominio.Interfaces;
using Dominio.Interfaces.InterfacesServicos;
using Entidades.Entidades;
using Entidades.Entidades.ViewModel;
using Entidades.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Servicos
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly IUsuario _IUsuario;

        public UsuarioServico(IUsuario usuario)
        {
            _IUsuario = usuario;
        }
        public async Task AdicionarUsuario(Usuario Usuario)
        {

               await _IUsuario.Adicionar(Usuario);
            
        }

        public async Task AtualizarUsuario(Usuario Usuario)
        {
   
                await _IUsuario.Atualizar(Usuario);
            
        }

        public List<ViewModelUsuario> BuscarUsuarioCustomizada()
        {
            var listaCustomizada = _IUsuario.ListaUsuarioCustomizada();

            var retorno = (from AutoEscola in listaCustomizada
                           select new ViewModelUsuario
                           {
                               Id = AutoEscola.Id,
                               NomeAutoEscola = AutoEscola.NomeEmpresa
                           }).ToList();
            return retorno;
        }

        public async Task<(bool Sucesso, string Mensagem)> CriarUsuario(Usuario registro,UserManager<Usuario> _userManager)
        {
            var user = new Usuario
            {
                UserName = registro.UserName,
                Email = registro.Email,
                Celular = registro.Celular,
                Tipo = TipoUsuario.Comum
            };

            var resultado = await _userManager.CreateAsync(user, registro.PasswordHash);

            if (resultado.Errors.Any())
            {
                return (false, "Erro ao criar usuário");
            }

            // Geração de Confirmação caso precise
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email 
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);
            if (resultado2.Succeeded)
                return (true, "Usuário registrado com sucesso");
            else
                return (false, "Erro ao confirmar email");
        }

        public async Task<bool> EmailPassowrdENulo(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return true;
            return false;
        }
        public async Task<Usuario> ValidarCredenciaisAsync(string email, string password, UserManager<Usuario> _userManager, SignInManager<Usuario> _signInManager)
        {
            if (await EmailPassowrdENulo(email, password))
                return null;
            if (!_userManager.Users.Any())
            {
                return null;
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null; // O usuário não foi encontrado
            var resultado = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!resultado.Succeeded)
                return null; // Autenticação falhou
            return user;
        }
#warning buscar Auto Escolas

        public List<Usuario> BuscarUsuarios()
        {
            throw new NotImplementedException();
        }
    }
}