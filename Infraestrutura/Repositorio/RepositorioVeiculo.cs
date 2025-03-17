using Dominio.Interfaces;
using Entidades.Entidades;
using InfraEstrutura.Configuracao;
using InfraEstrutura.Repositorio.Generico;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InfraEstrutura.Repositorio
{
    public class RepositorioVeiculo : RepositorioGenerico<Veiculo>, IVeiculo
    {
        private readonly DbContextOptions<Contexto> _optionsBuilder;
        public RepositorioVeiculo()
        {
            _optionsBuilder = new DbContextOptions<Contexto>();
        }

        public async Task<bool> ExisteVeiculo(string placa)
        {
            using (var banco = new Contexto(_optionsBuilder))
            {
                return await banco.veiculos
                    .Where(v => v.Placa.Equals(placa))
                    .AsTracking()
                    .AnyAsync();
            }
        }

        public async Task<List<Veiculo>> ListarVeiculos(Expression<Func<Veiculo, bool>> exVeiculo)
        {
            using (var banco = new Contexto(_optionsBuilder))
            {
                return await banco.veiculos.Where(exVeiculo).AsNoTracking().ToListAsync();
            }
        }

        public List<Veiculo> ListarVeiculosCustomizada(string idUsuario)
        {
            using (var banco = new Contexto(_optionsBuilder))
            {
                var listaVeiculos = (from Veiculo in banco.veiculos
                                     where Veiculo.IdUsuario == idUsuario
                                     select new Veiculo
                                     {
                                         Id = Veiculo.Id,
                                         Modelo = Veiculo.Modelo,
                                         Placa = Veiculo.Placa,
                                         KmAtual = Veiculo.KmAtual,
                                         KmTrocaOleo = Veiculo.KmTrocaOleo,
                                         IdUsuario = Veiculo.IdUsuario,

                                     }).AsNoTracking().ToList();
                return listaVeiculos;
            }
        }

    }
}