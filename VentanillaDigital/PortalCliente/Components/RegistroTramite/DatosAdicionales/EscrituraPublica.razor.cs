using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;
using System;
using System.Threading.Tasks;

namespace PortalCliente.Components.RegistroTramite.DatosAdicionales
{
    public partial class EscrituraPublica : ComponentBase
    {
        public string maxDate = DateTime.Now.ToString("yyyy-MM-dd");

        EscrituraPublicaDTO escrituraPublica = new EscrituraPublicaDTO()
        {
            FechaTramite = DateTime.Today
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Modify();
            }
        }

        [Parameter]
        public EventCallback<string> GetFields { get; set; }

        protected void oninput(ChangeEventArgs e)
        {
            Modify();
        }

        async void Modify()
        {
            string demo = JsonSerializer.Serialize(escrituraPublica);
            await GetFields.InvokeAsync(demo);
        }
    }
}
