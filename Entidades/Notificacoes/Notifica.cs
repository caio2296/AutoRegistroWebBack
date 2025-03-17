using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Notificacoes
{
    public class Notifica
    {
        public Notifica()
        {
            Notificacoes = new List<Notifica>();
        }
        [NotMapped]
        public string? NomeDaPropriedade { get; set; }
        [NotMapped]
        public string? Mensagem { get; set; }
        [NotMapped]
        public List<Notifica> Notificacoes { get; set; }
        public bool ValidarPropriedadeString(string valor, string nomeDaPropriedade)
        {
            if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomeDaPropriedade))
            {
                Notificacoes.Add(new Notifica()
                {
                    Mensagem = "Campo Obrigatório!",
                    NomeDaPropriedade = nomeDaPropriedade
                });
                return false;
            }
            return true;
        }
        public bool ValidarPropriedadeDecimal(decimal valor, string nomeDaPropriedade)
        {
            if (valor < 1 || string.IsNullOrWhiteSpace(nomeDaPropriedade))
            {
                Notificacoes.Add(new Notifica()
                {
                    Mensagem = "Campo Obrigatório!",
                    NomeDaPropriedade = nomeDaPropriedade

                });
                return false;
            }
            return true;
        }
    }
}
