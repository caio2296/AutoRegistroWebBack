using Aplicacao.Interfaces;
using Entidades.Entidades.ViewModel;
using Entidades.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AutoRegistroWebBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VeiculoController : Controller
    {

        private readonly IAplicacaoVeiculo _IaplicacaoVeiculo;
        public VeiculoController(IAplicacaoVeiculo aplicacaoVeiculo)
        {
            _IaplicacaoVeiculo = aplicacaoVeiculo;
        }
        [Authorize]
        [HttpGet("/api/BuscarPorId/{id}")]
        [Produces("application/json")]
        public async Task<ModelViewVeiculo> BuscarPorId(string id)
        {
            var veiculo = await _IaplicacaoVeiculo.BuscarPorId(id);
            var resultado = new ModelViewVeiculo
            {
                Id = veiculo.Id,
                Placa = veiculo.Placa
            };
            return resultado;
        }
        [Authorize]
        [HttpGet("/api/BuscarVeiculosCustomizada")]
        [Produces("application/json")]
        public List<ModelViewVeiculo> BuscarVeiculosCustomizada(string idAutoEscola)
        {
            return _IaplicacaoVeiculo.BuscarVeiculosCustomizada(idAutoEscola);
        }
        [Authorize]
        [HttpGet("/api/ExisteVeiculo")]
        [Produces("application/json")]
        public async Task<bool> ExisteVeiculo(string placa)
        {
            return await _IaplicacaoVeiculo.ExisteVeiculo(placa);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarVeiculo")]
        public void AdicionarVeiculo([FromBody] Veiculo veiculo)
        {
            _IaplicacaoVeiculo.Adicionar(veiculo);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPut("/api/AtualizarVeiculo")]
        public async Task AtualizarVeiculo([FromBody] Veiculo novoVeiculo)
        {
            await _IaplicacaoVeiculo.AtualizarVeiculo(novoVeiculo);
        }

        [Authorize]
        [Produces("application/json")]
        [HttpDelete("/api/ExcluirVeiculo")]
        public async Task ExcluirVeiculo([FromBody] Veiculo Objeto)
        {
           await _IaplicacaoVeiculo.Excluir(Objeto);
        }
    }
}


