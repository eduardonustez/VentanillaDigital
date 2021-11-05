using System.Threading.Tasks;
using ApiGateway.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalCliente.Data.Account;
using PortalCliente.Services.DescriptorCliente;
using PortalCliente.Services.Notario;

namespace PortalCliente.Services.RedireccionLogin
{

    public class RedireccionService : IRedireccionService
    {
        INotarioService _notarioService;

        AuthenticationStateProvider _authenticationStateProvider;

        NavigationManager _navigationManager;

        IDescriptorCliente _descriptorCliente;

        public RedireccionService(INotarioService notarioService,
            AuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager,
            IDescriptorCliente descriptorCliente)
        {
            _notarioService = notarioService;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
            _descriptorCliente = descriptorCliente;
        }

        public async Task IrAPaginaInicial()
        {
            var usuarioAutenticado = await ((CustomAuthenticationStateProvider)_authenticationStateProvider)?.GetAuthenticatedUser();
            string target = "login";
            bool esMovil = await _descriptorCliente.EsMovil;

            if (usuarioAutenticado != null)
            {
                if ((usuarioAutenticado.Rol == "Administrador" || usuarioAutenticado.Rol == "Notario Encargado") && !esMovil)
                {
                    EstadoPinFirmaModel estadoPinFirma = await _notarioService.ObtenerEstadoPinFirma(usuarioAutenticado.RegisteredUser.Email);
                    if (!(estadoPinFirma.FirmaRegistrada && estadoPinFirma.PinAsignado))
                    {
                        target = "configuracion";
                    }
                    else
                    {
                        target = "bandejaEntrada/2";
                    }
                }
                else
                {
                    target = "tramite";
                }
            }
            _navigationManager.NavigateTo(target);
        }
    }
}
