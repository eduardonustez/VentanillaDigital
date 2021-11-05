using ApiGateway.Models;
using PortalCliente.Data;
using PortalCliente.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalCliente.Services
{
    public interface IParametricasService
    {
        Task<TipoIdentificacion[]> ObtenerTiposIdentificacion();
        Task<ApiGateway.Models.City[]> GetCities();
        Task<Categoria[]> ObtenerCategorias(bool incluirTramitesDigitales = false);
        Task<AuthenticatedUser> GetAuthenticatedUser();
    }
}
