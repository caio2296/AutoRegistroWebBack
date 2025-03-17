using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades
{
    [Table("TB_Assinatura")]
    public class Assinatura
    {
        [Column("Id_Assinatura")]
        public string Id { get; set; }
        [Column("Status")]
        public string Status { get; set; }

        
        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
    }
}
