using Dominio.Interfaces.Generico;
using InfraEstrutura.Configuracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace InfraEstrutura.Repositorio.Generico
{
    public class RepositorioGenerico<T> : IGenerico<T>, IDisposable where T : class
    {
        private readonly DbContextOptions<Contexto> _optionBuilder;

        public RepositorioGenerico()
        {
            _optionBuilder = new DbContextOptionsBuilder<Contexto>()
                                .UseMySql("Server=mysql.autoregistro.kinghost.net;DataBase=autoregistro01;User=autoregistro01;Password=Zw4TH4jHDQt9nQU",
                                new MySqlServerVersion(new Version(10, 2, 36)))

            //.UseMySql("Server=localhost;DataBase=autoregistro;Uid=root;Pwd=zxcasd384!A",
            // new MySqlServerVersion(new Version(8, 0, 37)))
            //.UseSqlServer("Server=tcp:profilecaio.database.windows.net,1433;Initial Catalog=AutoRegistro;Persist Security Info=False;User ID=caio;Password=zxcasd384!A;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            .Options;
        }

        public async Task Adicionar(T Objeto)
        {
            try
            {
                using (var data = new Contexto(_optionBuilder))
                {
                    data.Set<T>().Add(Objeto);
                    await data.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        public async Task Atualizar(T Objeto)
        {
            using (var data = new Contexto(_optionBuilder))
            {
                data.Set<T>().Update(Objeto);
                await data.SaveChangesAsync();
            }
        }

        public async Task<T?> BuscarPorId(string id)
        {
            using (var data = new Contexto(_optionBuilder))
            {
                return await data.Set<T>()
                    .FindAsync(id);
            }
        }


        public async Task Excluir(T Objeto)
        {
            using (var data = new Contexto(_optionBuilder))
            {
                data.Set<T>().Remove(Objeto);
                await data.SaveChangesAsync();
            }
        }

        public async Task<List<T>> Listar()
        {
            using (var data = new Contexto(_optionBuilder))
            {
                return await data.Set<T>().AsNoTracking().ToListAsync();
            }
        }

        #region Disposed https://learn.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // To detect redundant calls
        private bool _disposedValue;

        // Instantiate a SafeHandle instance.
        private SafeHandle? _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle?.Dispose();
                    _safeHandle = null;
                }

                _disposedValue = true;
            }
        }
        #endregion
    }
}
