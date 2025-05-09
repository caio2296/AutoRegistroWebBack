using Entidades.Entidades;
using Entidades.Entidades.ViewModel;
namespace Dominio.Interfaces.InterfacesServicos
{
    public interface IVeiculoServico
    {
        Task AdicionarVeiculo(Veiculo veiculo);
        Task AtualizarVeiculo(Veiculo veiculo);
        Task<List<Veiculo>> BuscarVeiculosPertoTrocarOleo();
        Task<List<ModelViewVeiculo>> BuscarVeiculosCustomizada(string idUsuario);
    }
}
