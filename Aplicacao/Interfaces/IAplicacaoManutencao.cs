using Aplicacao.Interfaces.Generico;
using Entidades.Entidades;
using Entidades.Entidades.ViewModel;

namespace Aplicacao.Interfaces
{
    public interface IAplicacaoManutencao : IGenericoAplicacao<Manutencao>
    {
        Task AdicionarManutencao(Manutencao manutencao);
        Task AtualizarManutencao(Manutencao manutencao);
        Task<List<Manutencao>> BuscarManutencoes();
        Task<List<ViewModelManutencao>> BuscarManutencoesCustomizadas(string idVeiculo);
    }
}