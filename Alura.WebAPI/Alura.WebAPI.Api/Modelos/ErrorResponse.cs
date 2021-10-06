using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alura.WebAPI.Api.Modelos
{
    public class ErrorResponse
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public ErrorResponse InnerError { get; set; }
        public string[] Detalhes { get; set; }


        public static ErrorResponse From(Exception e)
        {
            if (e == null)
            {
                return null;
            }

            return new ErrorResponse
            {
                Codigo = e.HResult,
                Mensagem = e.Message,
                InnerError = From(e.InnerException),
            };
        }

        public static ErrorResponse FromModelState(ModelStateDictionary modelState)
        {
            //Coleta todos os erros na chamada;
            var erros = modelState.Values.SelectMany(m => m.Errors);

            //Constroi o objeto de resposta;
            return new ErrorResponse
            {
                Codigo = 100,
                Mensagem = "Houve erro(s) na requisição",
                Detalhes = erros.Select(e => e.ErrorMessage).ToArray(), //Dentro dos erros, obtenho a mensagem de cada um e após é convertido para Array;
            };
        }
    }
}
