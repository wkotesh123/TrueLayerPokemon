using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PokemonDex.Challenge.Exceptions;

namespace PokemonDex.Challenge.Middleware
{
    public class ExceptionHandlingMiddleWare : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsync("Pokemon Name not found");
                }
                else if (ex is ServerUnavaillableException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    await context.Response.WriteAsync("PokemonApi Service Unavailable");
                }
                else if (ex is PokemonNameEmptyException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync("Pokemon Name cannot be empty");
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(ex.Message); 
                }

            }
        }
    }
}
