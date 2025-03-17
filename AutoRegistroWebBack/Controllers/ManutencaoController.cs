using Aplicacao.Aplicacoes;
using Aplicacao.Interfaces;
using Entidades.Entidades;
using Entidades.Entidades.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegistro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManutencaoController:ControllerBase
    {
        private readonly IAplicacaoManutencao _IaplicacaoManutencao;
        public ManutencaoController(IAplicacaoManutencao aplicacaoManutencao)
        {
            _IaplicacaoManutencao = aplicacaoManutencao;
        }
        [Authorize]
        [HttpGet("/api/BuscarManutencoesCustomizadas")]
        [Produces("application/json")]
        public async Task<ActionResult<List<ViewModelManutencao>>> BuscarManutencoesCustomizadas(string idVeiculo)
        {
            return await _IaplicacaoManutencao.BuscarManutencoesCustomizadas(idVeiculo);
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarManutencao")]
        public async Task<ActionResult> AdicionarManutencao(Manutencao novaManutencao)
        {
            if (novaManutencao == null)
            {
                return BadRequest("Os dados da manutenção não podem ser nulos.");
            }

            try
            {
                // Chama o método de adicionar manutenção
                await _IaplicacaoManutencao.AdicionarManutencao(novaManutencao);

                // Retorna uma resposta de sucesso, com o código HTTP 201 (Created)
                return CreatedAtAction(nameof(AdicionarManutencao), new { id = novaManutencao.Id }, novaManutencao);
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna uma resposta de erro com status 500 (Internal Server Error)
                return StatusCode(500, $"Erro ao adicionar manutenção: {ex.Message}");
            }
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPut("/api/AtualizarManutencao")]
        public async Task<ActionResult> AtualizarManutencao(Manutencao novaManutencao)
        {
            if (novaManutencao == null)
            {
                return BadRequest("Os dados da manutenção não podem ser nulos.");
            }
            var manutencaoExistente = await _IaplicacaoManutencao.BuscarPorId(novaManutencao.Id);
            if (manutencaoExistente == null)
            {
                return NotFound($"Manutenção com ID {novaManutencao.Id} não encontrada.");
            }
            try
            {
                await _IaplicacaoManutencao.Atualizar(novaManutencao);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro ao atualizar manutenção: {ex.Message}");
            }
           
        }

        [Authorize]
        [Produces("application/json")]
        [HttpDelete("/api/ExcluirManutencao")]
        public async Task<ActionResult> ExcluirManutencao(Manutencao manutencao)
        {
            if (manutencao == null)
            {
                return BadRequest("Os dados da manutenção não podem ser nulos.");
            }
            var manutencaoExistente = await _IaplicacaoManutencao.BuscarPorId(manutencao.Id);
            if (manutencaoExistente == null)
            {
                return NotFound($"Manutenção com ID {manutencao.Id} não encontrada.");
            }
            try
            {
                await _IaplicacaoManutencao.Excluir(manutencao);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro ao excluir manutenção: {ex.Message}");
            }
           
        }
    }
}