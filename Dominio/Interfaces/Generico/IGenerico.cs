using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Generico
{
    public interface IGenerico<T> where T : class
    {
        Task Adicionar(T Objeto);
        Task Atualizar(T Objecto);
        Task Excluir(T Objeto);
        Task<T?> BuscarPorId(string id);
        Task<List<T>> Listar();
    }
}