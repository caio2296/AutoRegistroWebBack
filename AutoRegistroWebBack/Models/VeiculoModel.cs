namespace AutoRegistroWebBack.Models
{
    public class VeiculoModel
    {
        public string Id { get; set; }
        public string? Modelo { get; set; }
        public string Placa { get; set; }
        public int KmAtual { get; set; }
        public int KmTrocaOleo { get; set; }

    }
}
