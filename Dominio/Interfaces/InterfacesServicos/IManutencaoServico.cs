using Entidades.Entidades.ViewModel;
using Entidades.Entidades;
namespace Dominio.Interfaces.InterfacesServicos
{
    public interface IManutencaoServico
    {
        Task AdicionarManutencao(Manutencao manutencao);
        Task AtualizarManutencao(Manutencao manutencao);
        Task<List<Manutencao>> BuscarManutencoes();
        Task<List<ViewModelManutencao>> BuscarManutencoesCustomizadas(string idVeiculo);
    }
}
