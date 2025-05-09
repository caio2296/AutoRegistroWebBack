using Aplicacao.Interfaces;
using Entidades.Entidades.ViewModel;
using Entidades.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Numerics;
using AutoRegistroWebBack.Models;
using Dominio.Interfaces;

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
                Placa = veiculo.Placa,
                KmAtual = veiculo.KmAtual,
                KmTrocaOleo = veiculo.KmTrocaOleo
            };
            return resultado;
        }
        [Authorize]
        [HttpGet("/api/BuscarVeiculosCustomizada")]
        [Produces("application/json")]
        public async Task<IActionResult> BuscarVeiculosCustomizada()
        {
            try
            {
                var userId = User.FindFirst("idUsuario")?.Value;


                if (userId == null)
                    return BadRequest("Usuário não encontrado");
                if (await _IaplicacaoVeiculo.BuscarVeiculosCustomizada(userId) ==null)
                {
                    return NotFound("Veículos não encontrados.");
                }
                return Ok(await _IaplicacaoVeiculo.BuscarVeiculosCustomizada(userId));
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro no servidor! {ex.Message}");
            }
           
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarVeiculo")]
        public async  Task<IActionResult> AdicionarVeiculo([FromBody] VeiculoModel veiculo)
        {
            try
            {
                if (await _IaplicacaoVeiculo.ExisteVeiculo(veiculo.Placa))
                {
                    return BadRequest("Esse veículo já foi cadastrada");
                }
                else
                {
                    var userId = User.FindFirst("idUsuario")?.Value;

                    if (userId == null)
                        return BadRequest("Usuário não encontrado!");

                    var veiculoNovo = new Veiculo
                    {
                        Id = Guid.NewGuid().ToString(),
                        Modelo = veiculo.Modelo,
                        Placa = veiculo.Placa,
                        KmTrocaOleo = veiculo.KmTrocaOleo,
                        KmAtual = veiculo.KmAtual,
                        IdUsuario = userId
                    };
                    await _IaplicacaoVeiculo.Adicionar(veiculoNovo);
                    
                    return CreatedAtAction(nameof(veiculo),"Veículo", veiculoNovo);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            } 
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPut("/api/AtualizarVeiculo")]
        public async Task<IActionResult> AtualizarVeiculo([FromBody] VeiculoModel novoVeiculo)
        {
            try
            {
                var veiculoAtualizado = await _IaplicacaoVeiculo.BuscarPorId(novoVeiculo.Id);

                if (veiculoAtualizado == null)
                    return NotFound("Veículo não encontrado.");
                
                var userId = User.FindFirst("idUsuario")?.Value;

                if (userId == null)
                    return BadRequest("Usuário não encontrado!");

                var veiculo = new Veiculo
                {
                    Id = novoVeiculo.Id,
                    Placa = novoVeiculo.Placa,
                    KmAtual = novoVeiculo.KmAtual,
                    KmTrocaOleo = novoVeiculo.KmTrocaOleo,
                    IdUsuario = userId
                };
                await _IaplicacaoVeiculo.Atualizar(veiculo);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar veículo. {ex.Message}");
            }
        }
        [Authorize]
        [Produces("application/json")]
        [HttpDelete("/api/ExcluirVeiculo")]
        public async Task<IActionResult> ExcluirVeiculo([FromBody] Veiculo veiculo)
        {
            try
            {
                var veiculoDeletado = await _IaplicacaoVeiculo.BuscarPorId(veiculo.Id);

                if (veiculoDeletado == null)
                    return NotFound("Veículo não encontrado.");
                
                await _IaplicacaoVeiculo.Excluir(veiculoDeletado);
                return Ok("Veículo removido com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}


