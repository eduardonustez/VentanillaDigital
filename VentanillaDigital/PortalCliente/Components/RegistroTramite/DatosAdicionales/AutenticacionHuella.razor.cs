using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;
using System;

namespace PortalCliente.Components.RegistroTramite.DatosAdicionales
{
    public partial class AutenticacionHuella : ComponentBase
    {

        EnrolamientoNotariaDigitalDTO enrolamientoNotariaDigital = new EnrolamientoNotariaDigitalDTO()
        {
            Destinatario = "Notaria 16 del circuito de Bogotá"
        };
        [Parameter]
        public EventCallback<string> GetFields { get; set; }

        protected void oninput(ChangeEventArgs e)
        {
            Modify();
        }

        async void Modify()
        {
            string demo = JsonSerializer.Serialize(enrolamientoNotariaDigital);
            await GetFields.InvokeAsync(demo);
        }
    }
}