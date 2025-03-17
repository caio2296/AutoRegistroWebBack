using Entidades.Notificacoes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Entidades
{
    [Table("TB_Veiculo")]
    public class Veiculo : Notifica
    {
        public Veiculo()
        {
            Manutencoes = new List<Manutencao>();
        }
        [Column("SLT_ID")]
        public string Id { get; set; }
        [Column("SLT_Modelo")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Insira de 2 a 10 caracteres.")]
        public string Modelo { get; set; }
        [Column("SLT_Placa")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "Insira de 7 a 10 caracteres.")]
        public string Placa { get; set; }
        [Column("SLT_KmAtual")]
        [Range(0, 9999999)]
        public int KmAtual { get; set; }
        [Column("SLT_KmTrocaOleo")]
        [Range(0, 9999999)]
        public int KmTrocaOleo { get; set; }
        [Column("SLT_IdUsuario")]
        public string IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public List<Manutencao> Manutencoes { get; set; }
    }
}