using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.InterfacesServicos;
using Entidades.Entidades;
using Entidades.Entidades.ViewModel;

namespace Aplicacao.Aplicacoes
{
    public class AplicacaoManutencao : IAplicacaoManutencao
    {
        private IManutencaoServico _manutencaoServico;
        private IManutencao _manutencao;

        public AplicacaoManutencao(IManutencaoServico manutencaoServico, IManutencao manutencao)
        {
            _manutencaoServico = manutencaoServico;
            _manutencao = manutencao;
        }
        public async Task Adicionar(Manutencao Objeto)
        {
            await _manutencao.Adicionar(Objeto);
        }

        public async Task AdicionarManutencao(Manutencao manutencao)
        {
            await _manutencaoServico.AdicionarManutencao(manutencao);
        }

        public async Task Atualizar(Manutencao Objeto)
        {
            await _manutencao.Atualizar(Objeto);
        }

        public async Task AtualizarManutencao(Manutencao manutencao)
        {
            await _manutencaoServico.AtualizarManutencao(manutencao);
        }

        public async Task<List<Manutencao>> BuscarManutencoes()
        {
            return await _manutencaoServico.BuscarManutencoes();
        }

        public async Task<List<ViewModelManutencao>> BuscarManutencoesCustomizadas(string idVeiculo)
        {
            return await _manutencaoServico.BuscarManutencoesCustomizadas(idVeiculo);
        }

        public async Task<Manutencao> BuscarPorId(string id)
        {
            var manutencao = await _manutencao.BuscarPorId(id);

            return manutencao ?? new Manutencao();
        }

        public async Task Excluir(Manutencao Objeto)
        {
            if (Objeto == null)
            {
                // Lidar com a situação de objeto nulo, como lançar uma exceção ou apenas retornar
                throw new ArgumentNullException(nameof(Objeto), "O objeto não pode ser nulo.");
            }

            // Se não for nulo, tenta excluir
            await _manutencao.Excluir(Objeto);
        }

        public async Task<List<Manutencao>> Listar()
        {
            return await _manutencao.Listar();
        }
    }
}