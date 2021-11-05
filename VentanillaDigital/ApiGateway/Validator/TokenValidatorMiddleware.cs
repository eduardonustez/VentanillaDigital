using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Validator
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidatorMiddleware(RequestDelegate next)
        {
            _next = next;
           
        }
        public async Task Invoke(HttpContext context)
        {
            if ((context.Request.Headers.ContainsKey("Authorization") &&
                    context.Request.Headers["Authorization"][0].StartsWith("Bearer ")) == false)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("No autorizado!");
                return;

            }
            else
            {
                //setvalue of context
            }
            await _next.Invoke(context);
        }


    }
   
}
