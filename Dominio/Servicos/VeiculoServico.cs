using Dominio.Interfaces;
using Dominio.Interfaces.InterfacesServicos;
using Entidades.Entidades;
using Entidades.Entidades.ViewModel;

namespace Dominio.Servicos
{
    public class VeiculoServico : IVeiculoServico
    {
        private readonly IVeiculo _iVeiculos;
        public VeiculoServico(IVeiculo veiculo)
        {
            _iVeiculos = veiculo;
        }
        public async Task AdicionarVeiculo(Veiculo veiculo)
        {
            var validarModelo = veiculo.ValidarPropriedadeString(veiculo.Modelo, "Modelo");
            var validarPlaca = veiculo.ValidarPropriedadeString(veiculo.Placa, "Placa");
            var validarKmTrocaOleo = veiculo.ValidarPropriedadeString(veiculo.KmTrocaOleo.ToString(), "kmTrocaOleao");
            if (validarModelo && validarPlaca && validarKmTrocaOleo)
            {
               await _iVeiculos.Adicionar(veiculo);
            }
        }

        public async Task AtualizarVeiculo(Veiculo veiculo)
        {
            var validarModelo = veiculo.ValidarPropriedadeString(veiculo.Modelo, "Modelo");
            var validarPlaca = veiculo.ValidarPropriedadeString(veiculo.Placa, "Placa");
            var validarKmTrocaOleo = veiculo.ValidarPropriedadeString(veiculo.KmTrocaOleo.ToString(), "kmTrocaOleao");
            if (validarModelo && validarPlaca && validarKmTrocaOleo)
            {
               await _iVeiculos.Atualizar(veiculo);
            }
        }

        public async Task<List<Veiculo>> BuscarVeiculosPertoTrocarOleo()
        {
            return await _iVeiculos.ListarVeiculos(v => 2000 > (v.KmTrocaOleo - v.KmAtual));
        }

        public List<ModelViewVeiculo> BuscarVeiculosCustomizada(string idUsuario)
        {
            var listarVeiculosCustomizada = _iVeiculos.ListarVeiculosCustomizada(idUsuario);

            var retorno = (from Veiculo in listarVeiculosCustomizada
                           select new ModelViewVeiculo
                           {
                               Id = Veiculo.Id,
                               Modelo = Veiculo.Modelo,
                               Placa = Veiculo.Placa,
                               KmAtual = Veiculo.KmAtual,
                               KmTrocaOleo = Veiculo.KmTrocaOleo
                           }).ToList();
            return retorno;
        }
    }
}