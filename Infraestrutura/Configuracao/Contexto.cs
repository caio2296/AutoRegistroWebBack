using Entidades.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfraEstrutura.Configuracao
{
    public class Contexto : IdentityDbContext<Usuario>
    {
        public Contexto(DbContextOptions<Contexto> opcoes) : base(opcoes)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Veiculo> veiculos { get; set; }
        public DbSet<Manutencao> manutencao { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ObterStringConexaoMySql(),
                     new MySqlServerVersion(new Version(10, 2, 36)));
                //optionsBuilder.UseSqlServer(ObterStringConexao());
                base.OnConfiguring(optionsBuilder);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Assinatura>() // Configuração da entidade Assinatura
                    .HasOne(a => a.Usuario)  // Uma assinatura pertence a um usuário
                    .WithOne(u => u.Assinatura) // Um usuário tem uma assinatura
                    .HasForeignKey<Assinatura>(a => a.UsuarioId); // ✅ Agora correto!

            builder.Entity<Usuario>().ToTable("Usuario")
                .HasOne(u => u.Assinatura)
                .WithOne(a=> a.Usuario)
                .HasForeignKey<Usuario>(a => a.IdAssinatura)
                .IsRequired(false);

            builder.Entity<Veiculo>().ToTable("Veiculo")
                .HasOne(a => a.Usuario)
                .WithMany(v => v.Veiculos)
                .HasForeignKey(a => a.IdUsuario);
            builder.Entity<Manutencao>().ToTable("Manutencao")
                .HasOne(v => v.Veiculo)
                .WithMany(m => m.Manutencoes).HasForeignKey(m => m.IdVeiculo);
            base.OnModelCreating(builder);
        }

        private string ObterStringConexao()
        {
            string strcon =
                "Server = tcp:profilecaio.database.windows.net,1433; Initial Catalog = AutoRegistro; Persist Security Info = False; User ID = caio; Password =zxcasd384!A; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
            return strcon;
        }
        private string ObterStringConexaoMySql()
        {
            //string strcon = "Server=localhost;DataBase=autoregistro;Uid=root;Pwd=zxcasd384!A";
            string strcon = "Server=mysql.autoregistro.kinghost.net;DataBase=autoregistro;User=autoregistro;Password=zxcasd384";

            return strcon;
        }
    }
}