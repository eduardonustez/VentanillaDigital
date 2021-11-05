using ApiGateway.Models;
using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalAdministrador.Services
{
    public interface IParametricasService
    {
        Task<TipoIdentificacion[]> ObtenerTiposIdentificacion();
        Task<ApiGateway.Models.City[]> GetCities();
        Task<Categoria[]> ObtenerCategorias();
        Task<AuthenticatedUser> GetAuthenticatedUser();
    }
}
