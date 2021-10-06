using System;
using System.Collections.Generic;
using System.Linq;
using Alura.ListaLeitura.Modelos;

namespace Alura.WebAPI.Api.Modelos
{
    public static class LivroPaginaExtensions
    {
        public static LivroPaginado ToLivroPaginado(this IQueryable<LivroApi> query, LivroPaginacao paginacao)
        {
            var totalItens = query.Count();
            var totalPaginas = (int)Math.Ceiling(( totalItens / (double)paginacao.Tamanho ));


            return new LivroPaginado
            {
                Total = totalItens,
                TamanhoPagina = paginacao.Tamanho,
                TotalPaginas = totalPaginas,
                NumeroPagina = paginacao.Pagina,
                Anterior = ( paginacao.Pagina > 1 ) ? $"livros?tamanho={paginacao.Pagina - 1}&pagina={paginacao.Tamanho}" : "",
                Proximo = ( paginacao.Pagina < totalPaginas ) ? $"livros?tamanho={paginacao.Pagina + 1}&pagina={paginacao.Tamanho}" : "",

                Resultado = query
                        .Skip(( paginacao.Pagina - 1 ) * paginacao.Tamanho)
                        .Take(paginacao.Tamanho).ToList(),
            };
        }
    }

    public class LivroPaginado
    {
        public int Total { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroPagina { get; set; }
        public int TotalPaginas { get; set; }
        public IList<LivroApi> Resultado { get; set; }
        public string Anterior { get; set; }
        public string Proximo { get; set; }
    }

    public class LivroPaginacao
    {
        public int Pagina { get; set; } = 1;
        public int Tamanho { get; set; } = 25;
    }
}