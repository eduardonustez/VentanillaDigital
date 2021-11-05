using Microsoft.AspNetCore.Components;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;
using System;
using System.Threading.Tasks;

namespace PortalCliente.Components.RegistroTramite.DatosAdicionales
{
    public partial class InscripcionRegistroCivil : ComponentBase
    {
        InscripcionRegistroCivilDTO inscripcionRegCivil = new InscripcionRegistroCivilDTO();

        [Parameter]
        public EventCallback<string> GetFields { get; set; }

        private string[] TipoRegistroCivil = new string[3];
        protected override async Task OnInitializedAsync()
        {
            if (TipoRegistroCivil != null)
            {
                TipoRegistroCivil[0] = "Nacimiento";
                TipoRegistroCivil[1] = "Matrimonio";
                TipoRegistroCivil[2] = "Defunción";
            }
        }


        protected void oninput(ChangeEventArgs e)
        {
            Modify();
        }

        protected void onTipoRegistroSelected(ChangeEventArgs e)
        {
            inscripcionRegCivil.TipoRegistroCivil = e.Value.ToString();
            Modify();
        }

        async void Modify()
        {
            string demo = JsonSerializer.Serialize(inscripcionRegCivil);
            await GetFields.InvokeAsync(demo);
        }
    }
}