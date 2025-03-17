using Aplicacao.Interfaces.Generico;
using Entidades.Entidades;
using Entidades.Entidades.ViewModel;

namespace Aplicacao.Interfaces
{
    public interface IAplicacaoVeiculo : IGenericoAplicacao<Veiculo>
    {
        Task AdicionarVeiculo(Veiculo veiculo);
        Task<bool> ExisteVeiculo(string placa);
        Task AtualizarVeiculo(Veiculo veiculo);
        Task<List<Veiculo>> BuscarVeiculosPertoTrocarOleo();
        List<ModelViewVeiculo> BuscarVeiculosCustomizada(string idUsuario);
    }
}