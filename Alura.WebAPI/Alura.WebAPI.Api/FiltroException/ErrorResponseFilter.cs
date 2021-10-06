using Alura.WebAPI.Api.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alura.WebAPI.Api.FiltroException
{
    public class ErrorResponseFilter : IExceptionFilter
    {

        //Habilita o filtro para todas as exceptions não tratadas para este formato;
        public void OnException(ExceptionContext context)
        {
            var errorResponse = ErrorResponse.From(context.Exception);
            context.Result = new ObjectResult(errorResponse) { StatusCode = 500 };
        }
    }
}
