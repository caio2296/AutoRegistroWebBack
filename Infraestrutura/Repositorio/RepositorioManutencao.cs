using Dominio.Interfaces;
using Entidades.Entidades;
using InfraEstrutura.Configuracao;
using InfraEstrutura.Repositorio.Generico;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace InfraEstrutura.Repositorio
{
    public class RepositorioManutencao : RepositorioGenerico<Manutencao>, IManutencao
    {
        private readonly DbContextOptions<Contexto> _optionsBuilder;
        public RepositorioManutencao()
        {
            _optionsBuilder = new DbContextOptions<Contexto>();
        }
        public async Task<List<Manutencao>> ListarManutencoes(Expression<Func<Manutencao, bool>> exManutencao)
        {
            using (var banco = new Contexto(_optionsBuilder))
            {
                return await banco.manutencao.Where(exManutencao).AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<Manutencao>> ListarManutencoesCustomizada(string idVeiculo)
        {
            using (var banco = new Contexto(_optionsBuilder))
            {
                var listaManutencao = (from Manutencao in banco.manutencao
                                       where Manutencao.IdVeiculo == idVeiculo
                                       select new Manutencao
                                       {
                                           Id = Manutencao.Id,
                                           NomePeca = Manutencao.NomePeca,
                                           Fabricante = Manutencao.Fabricante,
                                           Preco = Manutencao.Preco,
                                           IdVeiculo = Manutencao.IdVeiculo,
                                           DataDaCompra = Manutencao.DataDaCompra,
                                           DataDaInstalacao = Manutencao.DataDaInstalacao,

                                       }).AsNoTracking().ToListAsync();
                return await listaManutencao;
            }
        }
    }
}