namespace AutoRegistroWebBack.Models
{
    public class RegistroModel
    {
        public string UserName { get; set; }= string.Empty;
        public string Email { get; set; } = string.Empty;   
        public string PasswordHash { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }    
        public string Celular { get; set; } = string.Empty;
        public string NomeEmpresa { get; set; } = string.Empty;
        public string UrlFoto { get; set; } = string.Empty;
    }
}
