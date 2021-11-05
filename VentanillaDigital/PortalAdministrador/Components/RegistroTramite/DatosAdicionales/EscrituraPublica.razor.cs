using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalAdministrador.Data.DatosTramite;
using System;
using System.Threading.Tasks;

namespace PortalAdministrador.Components.RegistroTramite.DatosAdicionales
{
    public partial class EscrituraPublica : ComponentBase
    {
        public string maxDate = DateTime.Now.ToString("yyyy-MM-dd");

        EscrituraPublicaDTO escrituraPublica = new EscrituraPublicaDTO()
        {
            FechaTramite = DateTime.Today
        };

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
