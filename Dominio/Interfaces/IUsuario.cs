using Dominio.Interfaces.Generico;
using Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IUsuario: IGenerico<Usuario>
    {
        public Task<bool> ExisteUsuario(string email, string senha);
        public Task<string> RetornarIdUsuario(string email);
        public List<Usuario> ListarUsuario(Expression<Func<Usuario, bool>> exUsuario);
        List<Usuario> ListaUsuarioCustomizada();
        Task<string> RetornarTipoUsuario(string email);
        Task<string> RetornarStatusAssinaturaUsuario(string email);


    }
}
