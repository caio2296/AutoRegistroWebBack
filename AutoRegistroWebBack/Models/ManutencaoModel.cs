using AutoRegistroWebBack.Validations;
using Entidades.Entidades;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace AutoRegistroWebBack.Models
{
    public class ManutencaoModel
    {
        public string? Id { get; set; }
        public string NomePeca { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "O preço não pode ser negativo.")]
        [MaxDecimalPlaces(2)]
        public decimal Preco { get; set; }
        public DateTime? DataDaCompra { get; set; }
        public DateTime? DataDaInstalacao { get; set; }
        public string Fabricante { get; set; }
        public string? IdVeiculo { get; set; }

        public static implicit operator Manutencao(ManutencaoModel request)
        {
            if (request.Id == null)
                return new Manutencao
                {
                    Id = Guid.NewGuid().ToString(),
                    NomePeca = request.NomePeca,
                    Preco = request.Preco,
                    DataDaCompra = request.DataDaCompra ?? new DateTime(2010, 1, 1),
                    DataDaInstalacao = request.DataDaInstalacao ?? new DateTime(2010, 1, 1),
                    Fabricante = request.Fabricante,
                    IdVeiculo = request.IdVeiculo
                };

            return new Manutencao
            {
                Id = request.Id,
                NomePeca = request.NomePeca,
                Preco = request.Preco,
                DataDaCompra = request.DataDaCompra ?? new DateTime(2010, 1, 1),
                DataDaInstalacao = request.DataDaInstalacao ?? new DateTime(2010, 1, 1),
                Fabricante = request.Fabricante,
                IdVeiculo = request.IdVeiculo
            };
        }

        //public static explicit operator Manutencao(ManutencaoModel request)
        //{
        //    if (request.Id == null)
        //        return new Manutencao
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            NomePeca = request.NomePeca,
        //            Preco = request.Preco,
        //            DataDaCompra = request.DataDaCompra ?? new DateTime(2010, 1, 1),
        //            DataDaInstalacao = request.DataDaInstalacao ?? new DateTime(2010, 1, 1),
        //            Fabricante = request.Fabricante,
        //            IdVeiculo = request.IdVeiculo
        //        };

        //    return new Manutencao
        //    {
        //        Id = request.Id,
        //        NomePeca = request.NomePeca,
        //        Preco = request.Preco,
        //        DataDaCompra = request.DataDaCompra ?? new DateTime(2010, 1, 1),
        //        DataDaInstalacao = request.DataDaInstalacao ?? new DateTime(2010, 1, 1),
        //        Fabricante = request.Fabricante,
        //        IdVeiculo = request.IdVeiculo
        //    };
        //}
    }
}
