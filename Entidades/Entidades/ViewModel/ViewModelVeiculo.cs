using System.ComponentModel.DataAnnotations;

namespace Entidades.Entidades.ViewModel
{
    public class ModelViewVeiculo
    {
        public string Id { get; set; }
        [StringLength(10, MinimumLength = 2)]
        public string Modelo { get; set; }
        [StringLength(10, MinimumLength = 7)]
        public string Placa { get; set; }
        [Range(0, 9999999)]
        public int KmAtual { get; set; }
        [Range(0, 9999999)]
        public int KmTrocaOleo { get; set; }
        public int IdAutoEscola { get; set; }
    }
}
