using Entidades.Entidades;
using Entidades.Entidades.ViewModel;

namespace Dominio.Interfaces.InterfacesServicos
{
    public interface IUsuarioServico
    {
        Task AdicionarUsuario(Usuario Usuario);
        Task AtualizarUsuario(Usuario Usuario);
        List<Usuario> BuscarUsuarios();
        List<ViewModelUsuario> BuscarUsuarioCustomizada();
    }
}
