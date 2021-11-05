using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalAdministrador.Data.DatosTramite;
using System;

namespace PortalAdministrador.Components.RegistroTramite.DatosAdicionales
{
    public partial class AutenticacionFirma : ComponentBase
    {

        AutenticacionFirmaDTO documentoPrivado = new AutenticacionFirmaDTO();
        [Parameter]
        public EventCallback<string> GetFields { get; set; }

        protected void oninput(ChangeEventArgs e)
        {
            Modify();
        }

        async void Modify()
        {
            string demo = JsonSerializer.Serialize(documentoPrivado);
            await GetFields.InvokeAsync(demo);
        }
    }
}