using ApiGateway.Contratos.Models.Notario;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;
using PortalAdministrador.Services;
using PortalAdministrador.Services.Notario;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Pages.NotarioPages
{
    public partial class Configuraciones:ComponentBase
    {
        [Inject]
        protected INotarioService NotarioService { get; set; }
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]
        private IConfiguracionesService _configuracionesService { get; set; }

        [Inject]
        NotificationService notificationService { get; set; }

        string UsarSticker;
        long notarioId;
        string NoUsarTableta;
        string MostrarATDP;
        
        string UserEmail = "";
        List<NotarioReturnDTO> notarioReturnDTOs;
        bool UsarFirmaManual;



        int tipoIdentificacion = 0;
        protected override async Task OnInitializedAsync()
        {
            //var userActual = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            //AuthenticatedUser userName = await userActual.GetAuthenticatedUser();
            //UserEmail = userName.RegisteredUser.UserName;
            //var opcionesConfiguracion = await NotarioService.ObtenerOpcionesConfiguracion(UserEmail);
            //if (opcionesConfiguracion.UsarSticker)
            //    UsarSticker = "1";
            //else
            //    UsarSticker = "0";
            var opc_conf = await _configuracionesService.ObtenerOpcionesConfiguracion();
            notarioReturnDTOs = await _configuracionesService.ObtenerNotariosNotaria();
            notarioId = notarioReturnDTOs.Where(n => n.NotarioDeTurno == true).FirstOrDefault().NotarioId;
            if (opc_conf.UsarSticker)
                UsarSticker = "1";
            else
                UsarSticker = "0";

            UsarFirmaManual = opc_conf.FirmaManual;

        }
        public async Task Guardar()
        {
            await _configuracionesService.GuardarOpcionesConfiguracion(new OpcionesConfiguracion()
            {
                FirmaManual = UsarFirmaManual,
                UsarSticker = UsarSticker == "1" ? true : false
            });
            await _configuracionesService.SeleccionarNotarioNotaria(new NotarioNotariaDTO() { NotarioId = notarioId });
            ShowNotification();
        }
        async void ShowNotification()
        {
            var message = new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Configuración guardada",
                Detail = "Se han almacenado sus preferencias.",
                Duration = 7000
            };
            notificationService.Notify(message);
        }

        void UsarStickerCheck (object checkedValue)
        {
            if ((bool)checkedValue)
                UsarSticker = "1";
            else
                UsarSticker = "0";
            Console.WriteLine("Usa sticker:" + UsarSticker);
        }


        void ActivarFirmaManualCheck(object checkedValue)
        {
            UsarFirmaManual = (bool)checkedValue;
        }
    }
}
