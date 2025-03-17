using Entidades.Enums;
using Entidades.Notificacoes;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Entidades
{
    [Table("TB_Usuario")]
    public class Usuario :IdentityUser
    {
        [Column("USR_NomeAutoEscola")]
        public string? NomeAutoEscola { get; set; }
        [Column("USR_URLFOTO")]
        public string? UrlFoto { get; set; }
        [Column("USR_CELULAR")]
        public string? Celular { get; set; }
        [Column("USR_TIPO")]
        public TipoUsuario? Tipo { get; set; }
        [Column("IdAssinatura")]
        [ForeignKey("IdAssinatura")]
        public string IdAssinatura { get; set; }
        public Assinatura Assinatura { get; set; }
        public List<Veiculo> Veiculos { get; set;}

    }
}
