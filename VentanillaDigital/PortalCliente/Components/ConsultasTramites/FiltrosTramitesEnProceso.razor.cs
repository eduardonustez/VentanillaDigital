using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Infraestructura.Transversal.Models;
namespace PortalCliente.Components.ConsultasTramites
{
    public partial class FiltrosTramitesEnProceso : ComponentBase
    { 
        [Inject]
        IJSRuntime Js { get; set; }

        [Parameter]
        public EventCallback<List<Filtro>> FiltrosChanged { get; set; }
        [Parameter]
        public string UserName{get;set;}

         async void MostrarFiltros(bool mostrarOverlay = true)
        {
            await Js.InvokeVoidAsync("mostrarFiltros", mostrarOverlay);
        }
        async void Filtrar(string autorTramite)
        {
            var Filtros = new List<Filtro>();
            if(autorTramite=="1")
                Filtros.Add(new Filtro("Tramites.UsuarioCreacion", UserName));
            await FiltrosChanged.InvokeAsync(Filtros);
            MostrarFiltros(false);
        }
    }
}