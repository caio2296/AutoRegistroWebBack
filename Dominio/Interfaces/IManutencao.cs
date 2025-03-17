using Dominio.Interfaces.Generico;
using Entidades.Entidades;
using System.Linq.Expressions;

namespace Dominio.Interfaces
{
    public interface IManutencao : IGenerico<Manutencao>
    {
        Task<List<Manutencao>> ListarManutencoes(Expression<Func<Manutencao, bool>> exManutencao);
        Task<List<Manutencao>> ListarManutencoesCustomizada(string idVeiculo);
    }
}