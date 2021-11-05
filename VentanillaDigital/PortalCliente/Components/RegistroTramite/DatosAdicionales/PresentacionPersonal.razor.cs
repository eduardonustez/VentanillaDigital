using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;
using System;

namespace PortalCliente.Components.RegistroTramite.DatosAdicionales
{
    public partial class PresentacionPersonal : ComponentBase
    {

        PresentacionPersonalDTO presentacionPersonal = new PresentacionPersonalDTO();
        [Parameter]
        public EventCallback<string> GetFields { get; set; }

        protected void oninput(ChangeEventArgs e)
        {
            Modify();
        }

        async void Modify()
        {
            string demo = JsonSerializer.Serialize(presentacionPersonal);
            await GetFields.InvokeAsync(demo);
        }
    }
}