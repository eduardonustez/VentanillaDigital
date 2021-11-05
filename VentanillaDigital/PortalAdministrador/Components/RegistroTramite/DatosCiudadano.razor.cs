using Microsoft.AspNetCore.Components.Forms;
using PortalAdministrador.Data;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using PortalAdministrador.Helper;
using Microsoft.AspNetCore.Components.Web;

namespace PortalAdministrador.Components.RegistroTramite
{
    public partial class DatosCiudadano : ComponentBase
    {
        [Inject]
        protected Services.IParametricasService GeneralService { get; set; }

        [Inject]
        protected Services.ITramiteService TramiteService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public PersonaInfo PersonaInfo { get; set; }

        [Parameter]
        public EventCallback<PersonaInfo> PersonaInfoChanged { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        public string InputID = "input-id";

        string CurrTipoDoc { get; set; }

        string fueraDespacho;
        private async Task ChildChanged(string property, object arg)
        {
            switch (property)
            {
                case "TipoIdentificacion":
                    CurrTipoDoc = (string)arg;
                    PersonaInfo.TipoIdentificacion = _tiposIdentificaciones.FirstOrDefault(t => t.Abreviatura == CurrTipoDoc);
                    break;
                case "NumeroIdentificacion":
                    PersonaInfo.NumeroIdentificacion = ((string)arg).ToUpper().Trim();
                    break;
                case "Nombres":
                    PersonaInfo.Nombres = ((string)arg).ToUpper().Trim();
                    break;
                case "Apellidos":
                    PersonaInfo.Apellidos = ((string)arg).ToUpper().Trim();
                    break;
                case "NumeroCelular":
                    PersonaInfo.NumeroCelular = (string)arg;
                    break;
                case "Email":
                    PersonaInfo.Email = ((string)arg).ToUpper();
                    break;
            }
            if (PersonaInfo.TipoIdentificacion.Abreviatura == "TI" || PersonaInfo.TipoIdentificacion.Abreviatura == "CC")
            {
                Regex regex = new Regex("[0-9]");
                if (!regex.IsMatch(PersonaInfo.NumeroIdentificacion))
                {
                    PersonaInfo.IsValid(false);
                }
            }
            await PersonaInfoChanged.InvokeAsync(PersonaInfo);
            await ReadyChanged.InvokeAsync(PersonaInfo.IsValid());
        }


        private string LoginMessage;
        private TipoIdentificacion[] _tiposIdentificaciones;

        private async Task EstablecerTipoDocumentoDefecto()
        {
            if (_tiposIdentificaciones != null && _tiposIdentificaciones.Length > 0)
            {
                if (PersonaInfo != null && PersonaInfo.TipoIdentificacion == null)
                {
                    PersonaInfo.TipoIdentificacion = _tiposIdentificaciones[0];
                    await PersonaInfoChanged.InvokeAsync(PersonaInfo);
                    await ReadyChanged.InvokeAsync(PersonaInfo.IsValid());
                }
            }
        }

        protected async override Task OnInitializedAsync()
        {
            _tiposIdentificaciones = await GeneralService.ObtenerTiposIdentificacion();
            Console.WriteLine(_tiposIdentificaciones);
            await EstablecerTipoDocumentoDefecto();
        }

        protected async override Task OnParametersSetAsync()
        {
            if (PersonaInfo == null)
            {
                PersonaInfo = new PersonaInfo();
                await PersonaInfoChanged.InvokeAsync(PersonaInfo);
            }
            await EstablecerTipoDocumentoDefecto();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && PersonaInfo != null)
            {
                await Focus();
                await ReadyChanged.InvokeAsync(PersonaInfo.IsValid());
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Focus()
        {
            await JSRuntime.InvokeVoidAsync("focusInput", InputID);
        }

        private async Task<bool> EscanearDocumento()
        {
            Random rnd = new Random();
            int numeroDoc = rnd.Next(1010000000, 1070999999);

            PersonaInfo.Apellidos = generarApellidos();
            PersonaInfo.Nombres = generarNombres();
            PersonaInfo.NumeroIdentificacion =
                rnd.Next(1010000000, 1070999999).ToString();
            PersonaInfo.TipoIdentificacion = _tiposIdentificaciones[0];
            PersonaInfo.Email = "jaime.silva@olimpiait.com";
            PersonaInfo.NumeroCelular = "3115801000";
            LoginMessage = null;
            await PersonaInfoChanged.InvokeAsync(PersonaInfo);
            return true;
        }

        private String generarNombres()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            String[] nombres = { "Juan", "Pablo", "Pedro", "Jose", "Alberto", "Daniel", "Luis", "Carlos", "Andres", "Alfonso" };
            String gen = nombres[rnd.Next(0, 10)] + " " + nombres[rnd.Next(0, 10)];
            return gen;
        }
        private String generarApellidos()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            String[] nombres = { "Rodriguez", "Mora", "Cadena", "Ramirez", "Perez", "Doe", "Medina", "Suarez", "Quijano", "Porras" };
            String gen = nombres[rnd.Next(0, 10)] + " " + nombres[rnd.Next(0, 10)];
            return gen;
        }

        void TramiteEnNotaria(object checkedValue)
        {
            fueraDespacho = "0";
        }
        void TramiteFueraDespacho(object checkedValue)
        {
            fueraDespacho = "1";
        }
    }
}
