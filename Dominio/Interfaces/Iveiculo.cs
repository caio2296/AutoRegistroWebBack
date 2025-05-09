using Dominio.Interfaces.Generico;
using Entidades.Entidades;
using System.Linq.Expressions;
namespace Dominio.Interfaces
{
    public interface IVeiculo : IGenerico<Veiculo>
    {
        Task<List<Veiculo>> ListarVeiculos(Expression<Func<Veiculo, bool>> exVeiculo);
        Task<List<Veiculo>> ListarVeiculosCustomizada(string idUsuario);
        Task<bool> ExisteVeiculo(string placa);
    }
}
