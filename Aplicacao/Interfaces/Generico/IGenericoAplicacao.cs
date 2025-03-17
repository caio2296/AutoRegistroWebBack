namespace Aplicacao.Interfaces.Generico
{
    public interface IGenericoAplicacao<T> where T : class
    {
        Task Adicionar(T Objeto);
        Task Atualizar(T Objeto);
        Task Excluir(T Objeto);
        Task<T> BuscarPorId(string id);
        Task<List<T>> Listar();
    }
}
