using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalAdministrador.Data.DatosTramite;
using System;

namespace PortalAdministrador.Components.RegistroTramite.DatosAdicionales
{
    public partial class DocumentoPrivado : ComponentBase
    {

        DocumentoPrivadoDTO documentoPrivado = new DocumentoPrivadoDTO();
        [Parameter]
        public EventCallback<string> GetFields { get; set; }

        protected void oninput(ChangeEventArgs e)
        {
            Modify();
        }

        private async void Modify()
        {
            string demo = JsonSerializer.Serialize(documentoPrivado);
            await GetFields.InvokeAsync(demo);
        }
    }
}