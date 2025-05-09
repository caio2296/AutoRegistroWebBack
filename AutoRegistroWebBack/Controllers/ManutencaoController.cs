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
using AutoRegistroWebBack.Models;

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
        [HttpGet("/api/BuscarManutencoesCustomizadas/{idVeiculo}")]
        [Produces("application/json")]
        public async Task<ActionResult<List<ViewModelManutencao>>> BuscarManutencoesCustomizadas(string idVeiculo)
        {
            try
            {
                if (await _IaplicacaoManutencao.BuscarManutencoesCustomizadas(idVeiculo) == null)
                {
                    return NotFound("Veículos não encontrados.");
                }
                return Ok(await _IaplicacaoManutencao.BuscarManutencoesCustomizadas(idVeiculo));
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro no servidor. {ex.Message}");
            }
           
        }
        [Authorize]
        [HttpGet("/api/BuscarManutencaoId/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ViewModelManutencao>> BuscarManutencaoId(string id)
        {
            try
            {
                if (await _IaplicacaoManutencao.BuscarPorId(id) == null)
                   return NotFound("Manutenção não encontrada!");

                return Ok(await _IaplicacaoManutencao.BuscarPorId(id));
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro no servidor. {ex.Message}");
            }
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarManutencao")]
        public async Task<ActionResult> AdicionarManutencao([FromBody]ManutencaoModel novaManutencao)
        {
            if (novaManutencao == null)
            {
                return BadRequest("Os dados da manutenção não podem ser nulos.");
            }

            try
            {
                Manutencao manutencao = novaManutencao;
                // Chama o método de adicionar manutenção
                await _IaplicacaoManutencao.AdicionarManutencao(manutencao);

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
        [HttpPatch("/api/AtualizarManutencao")]
        public async Task<ActionResult> AtualizarManutencao(ManutencaoModel novaManutencao)
        {
            try
            {
                if (novaManutencao.Id == null)
            {
                return BadRequest("Os dados da manutenção não podem ser nulos.");
            }
            var manutencaoExistente = await _IaplicacaoManutencao.BuscarPorId(novaManutencao.Id);
            if (manutencaoExistente == null)
            {
                return NotFound($"Manutenção com ID {manutencaoExistente.Id} não encontrada.");
            }
                novaManutencao.IdVeiculo = manutencaoExistente.IdVeiculo;
                Manutencao manutencao = novaManutencao;
                await _IaplicacaoManutencao.Atualizar(manutencao);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro ao atualizar manutenção: {ex.Message}");
            }
           
        }

        [Authorize]
        [Produces("application/json")]
        [HttpDelete("/api/ExcluirManutencao/{id}")]
        public async Task<ActionResult> ExcluirManutencao(string id)
        {
            var manutencaoExistente = await _IaplicacaoManutencao.BuscarPorId(id);
            if (manutencaoExistente == null)
            {
                return NotFound($"Manutenção com ID {manutencaoExistente} não encontrada.");
            }
            try
            {
                await _IaplicacaoManutencao.Excluir(manutencaoExistente);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro ao excluir manutenção: {ex.Message}");
            }
           
        }
    }
}