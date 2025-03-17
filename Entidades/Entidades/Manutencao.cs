using Entidades.Notificacoes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Entidades
{
    [Table("TB_Manutencao")]
    public class Manutencao : Notifica
    {


        [Column("SLT_ID")]
        public string Id { get; set; }
        [Column("SLT_NomePeca")]
        public string NomePeca { get; set; }
        [Column("SLT_Preco")]
        public decimal Preco { get; set; }

        [Column("SLT_DataDaCompra")]
        public DateTime DataDaCompra { get; set; }
        [Column("SLT_DataDaInstalacao")]
        public DateTime DataDaInstalacao { get; set; }
        [Column("SLT_Fabricante")]
        public string Fabricante { get; set; }

        [Column("SLT_IdVeiculo")]
        public string IdVeiculo { get; set; }

        public Veiculo Veiculo { get; set; }

    }
}