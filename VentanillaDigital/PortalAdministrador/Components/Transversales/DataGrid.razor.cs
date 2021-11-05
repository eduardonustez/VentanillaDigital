using ApiGateway.Models.Transaccional;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace PortalAdministrador.Components.Transversales
{
    public partial class DataGrid : ComponentBase
    {

        [Inject]
        IJSRuntime Js { get; set; }

        [Parameter]
        public object[,] data { get; set; }
        [Parameter]
        public string[] columnTitles { get; set; }

        [Parameter]
        public int registrosPagina { get; set; }

        [Parameter]
        public bool mostrarCheckboxes { get; set; }

        [Parameter]
        public int totalRegistros { get; set; }
        [Parameter]
        public int totalPaginas { get; set; }
        [Parameter]
        public string TipoTramite { get; set; }
        [Parameter]
        public EventCallback<int> OnChangePage { get; set; }

        public bool mostrarMensajeFirmando = false;

        [Parameter]
        public EventCallback<string> OnSelectRow { get; set; }

        public List<long> consecMultiples = new List<long>();

        int indicePagina;

        bool todosSeleccionados = false;

        string selected = "";

        protected override async Task OnInitializedAsync()
        {
            indicePagina = 1;

        }

        protected override async Task OnParametersSetAsync()
        {
            mostrarMensajeFirmando = false;
            if (TipoTramite == "3")
            {
                if (data != null)
                {
                    for (int i = 0; i < data.GetLength(0); i++)
                    {
                        if (data[i, 5].ToString() == "Firmando")
                        {
                            mostrarMensajeFirmando = true;
                            break;
                        }
                    }
                }
            }
        }

        public void ResetIndex()
        {
            OnClickFirst();
        }

        async void OnClickFirst()
        {
            indicePagina = 1;
            await OnChangePage.InvokeAsync(indicePagina);
        }

        async void OnClickPrevious()
        {
            if (indicePagina > 1)
            {
                indicePagina--;
                await OnChangePage.InvokeAsync(indicePagina);
            }
        }
        async void OnClickNext()
        {
            if (indicePagina < totalPaginas)
            {
                indicePagina++;
                await OnChangePage.InvokeAsync(indicePagina);
            }
        }
        async void OnClickLast()
        {
            indicePagina = totalPaginas;
            await OnChangePage.InvokeAsync(indicePagina);
        }

        async void AgregarSeleccion(string id)
        {
            if (consecMultiples.Contains(long.Parse(id)))
            {
                consecMultiples.Remove(long.Parse(id));
            }
            else
            {
                consecMultiples.Add(long.Parse(id));
            }
            await Js.InvokeVoidAsync("verBotones");
        }

        async void SeleccionarTodos()
        {
            todosSeleccionados = !todosSeleccionados;
            if (todosSeleccionados)
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    consecMultiples.Add(long.Parse(data[i, 0].ToString()));
                }
            }
            else
            {
                consecMultiples = new List<long>();
            }

            await Js.InvokeVoidAsync("seleccionarTodos");
        }

        public async void DeSeleccionarTodos()
        {
            todosSeleccionados = false;
            if (todosSeleccionados)
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    consecMultiples.Add(long.Parse(data[i, 0].ToString()));
                }
            }
            else
            {
                consecMultiples = new List<long>();
            }

            await Js.InvokeVoidAsync("seleccionarTodos");
        }
    }
}
