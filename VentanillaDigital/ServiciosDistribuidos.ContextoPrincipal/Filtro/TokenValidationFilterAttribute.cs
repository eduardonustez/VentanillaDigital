using Aplicacion.ContextoPrincipal.Modelo;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Excepcion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ServiciosDistribuidos.ContextoPrincipal.Filtro
{
    public class TokenValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly AuthContext _authContext;
        public TokenValidationFilterAttribute(AuthContext authContext)
        {
            _authContext = authContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryString = context.HttpContext.Request.Query;
            StringValues userId;
            StringValues token;
            bool IsTokenValid = false;
            if (queryString.TryGetValue("userId", out userId) &&
                queryString.TryGetValue("token", out token))
            {
                IsTokenValid = _authContext.UserTokens.Any(u => u.UserId == (string)userId && u.Value == (string)token && u.Name == "Token");

            }
            if (!IsTokenValid)
            {
                string[] errores = { "Invalid Token" };
                context.Result = new ConflictObjectResult(new ErrorDTO() { Errors = errores });
            }

        }

    }

    public class TokenValidationAdministrationAttribute : ActionFilterAttribute
    {
        private readonly AuthContext _authContext;

        public TokenValidationAdministrationAttribute(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryString = context.HttpContext.Request.Query;
            StringValues userId;
            StringValues token;
            bool IsTokenValid = false;
            //if (queryString.TryGetValue("userId", out userId) &&
            //    queryString.TryGetValue("token", out token))
            if (queryString.TryGetValue("token", out token))
            {
                //IsTokenValid = _authContext.UserTokens.Any(u => u.UserId == (string)userId && u.Value == (string)token && u.Name == "Token");
            }

            if (string.IsNullOrEmpty(token.ToString()))
            {
                context.Result = new ConflictObjectResult(new ErrorDTO() { Errors = new[] { "Invalid Token" } });
            }

            //if (!IsTokenValid)
            //{
            //    string[] errores = { "Invalid Token" };
            //    context.Result = new ConflictObjectResult(new ErrorDTO() { Errors = errores });
            //}
        }
    }

    public class TokenValidationAdministrationRoleAttribute : ActionFilterAttribute
    {
        private readonly AuthContext _authContext;

        public TokenValidationAdministrationRoleAttribute(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryString = context.HttpContext.Request.Query;
            StringValues token;

            if (queryString.TryGetValue("token", out token))
            {
                if (!ValidateRolToken(token))
                {
                    context.Result = new ConflictObjectResult(new ErrorDTO() { Errors = new[] { "El usuario no es administrador" } });
                    return;
                }
            }

            if (string.IsNullOrEmpty(token.ToString()))
            {
                context.Result = new ConflictObjectResult(new ErrorDTO() { Errors = new[] { "Invalid Token" } });
            }
        }

        private bool ValidateRolToken(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (!jwtSecurityTokenHandler.CanReadToken(token))
            {
                return false;
            }

            var jwtSecurityToken = jwtSecurityTokenHandler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            if (!jwtSecurityToken.Payload.Any(m => m.Key == ClaimTypes.Role && m.Value.ToString().Equals("ADMIN")))
            {
                return false;
            }

            return true;
        }
    }
}
