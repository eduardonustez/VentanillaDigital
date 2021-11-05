using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;
using System;

namespace PortalCliente.Components.RegistroTramite.DatosAdicionales
{
    public partial class DocumentoPrivadoInvidente : ComponentBase
    {

        DocumentoPrivadoInvidenteDTO documentoPrivado = new DocumentoPrivadoInvidenteDTO();

        bool NoSabeFirmar { get; set; }

        [Parameter]
        public EventCallback<string> GetFields { get; set; }

        [Parameter]
        public EventCallback<bool> NoSabeFirmarChanged { get; set; }

        protected override void OnInitialized()
        {
            documentoPrivado.SabeFirmar = false;
        }
        protected void oninput(ChangeEventArgs e)
        {
            Modify();
        }

        async void Modify()
        {
            documentoPrivado.SabeFirmar = !NoSabeFirmar;
            string demo = JsonSerializer.Serialize(documentoPrivado);
            await GetFields.InvokeAsync(demo);
            await NoSabeFirmarChanged.InvokeAsync(documentoPrivado.SabeFirmar);
        }
    }
}