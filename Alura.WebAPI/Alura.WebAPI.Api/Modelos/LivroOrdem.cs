using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core; // Importe para utilizar o OrderBy
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;

namespace Alura.WebAPI.Api.Modelos
{
    public static class LivroOrdemExtensions
    {
        public static IQueryable<Livro> AplicaOrdem (this IQueryable<Livro> query, LivroOrdem ordem )
        {
            if (ordem.OrdernarPor != null)
            {
                query = query.OrderBy(ordem.OrdernarPor);
            }
            return query;
        }
    }

    public class LivroOrdem
    {
        public string OrdernarPor { get; set; }
    }
}
