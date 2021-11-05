using ApiGateway.Contratos.Models;
using ApiGateway.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PortalAdministrador.Components.Shared;
using PortalAdministrador.Services;
using PortalAdministrador.Services.Notaria;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalAdministrador.Pages.Diagnostico
{
    public partial class ConfiguracionesMaquina : ComponentBase
    {
        [Inject]
        IJSRuntime Js { get; set; }

        [Inject]
        IMachineService _MachineService { get; set; }

        [Inject]
        INotariaService _NotariaService { get; set; }

        public string CorreoFilter { get; set; }
        public GridControl<MaquinaConfiguracionReturn> Grid { get; set; }
        public bool consultando { get; set; }
        public List<MaquinaConfiguracionReturn> Usuarios { get; set; }
        public int TotalPages { get; set; }
        public long TotalRows { get; set; }
        public List<(string, string)> Columns { get; set; } = new List<(string, string)>
        {
            ("Correo", "CorreoUsuario"),
            ("IP", "DireccionIP"),
            ("Fecha acceso", "FechaModificacion"),
            ("Mac", "MAC"),
            ("Canal Wacom", "CanalWacom"),
            ("Estado WacomSigCaptX", "EstadoWacomSigCaptX"),
            ("Estado Dll Wacom", "EstadoDllWacom")
        };
        public long NotariaSeleccionada { get; set; }
        public IEnumerable<NotariaBasicModel> Notarias { get; set; }
        PaginableResponse<MaquinaConfiguracionReturn> result { get; set; }



        protected override async Task OnInitializedAsync()
        {
            Notarias = await _NotariaService.ObtenerNotariasAsync();
        }

        public async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await Filtrar();
            }
        }

        public async Task LimpiarCampos()
        {
            CorreoFilter = string.Empty;
            NotariaSeleccionada = 0;
            await ConsultarMaquinas();
        }

        public async Task Filtrar()
        {
            await ConsultarMaquinas();
            Grid.ResetIndex();
            //await MostrarFiltros(false);
        }


        public async Task ConsultarMaquinas()
        {
            consultando = true;

            var request = new ConfiguracionesNotariaRequest
            {
                CorreoUsuario = CorreoFilter,
                NotariaId = NotariaSeleccionada
            };
            result = await _MachineService.ObtenerConfiguracionesMaquina(request);
            if (result != null)
            {
                Usuarios = (List<MaquinaConfiguracionReturn>)result.Data;
                TotalRows = result.TotalRows;
                TotalPages = 1;
            }

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result.Data));

            consultando = false;

            StateHasChanged();
        }

        public async Task MostrarFiltros(bool mostrarOverlay = true)
        {
            await Js.InvokeVoidAsync("mostrarFiltros", mostrarOverlay);
        }

        public void Refrescar()
        {
            CorreoFilter = string.Empty;
            NotariaSeleccionada = 0;
            Grid.ResetIndex();
            //await LimpiarCampos();
        }

        public async void OnChangePage(int indice)
        {
            await ConsultarMaquinas();
        }

    }
}