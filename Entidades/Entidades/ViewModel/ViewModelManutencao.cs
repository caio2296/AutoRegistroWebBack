namespace Entidades.Entidades.ViewModel
{
    public class ViewModelManutencao
    {
        public string Id { get; set; }
        public string? NomePeca { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataDaCompra { get; set; }
        public DateTime DataDaInstalacao { get; set; }
        public string? Fabricante { get; set; }
        public string IdVeiculo { get; set; }
    }
}
