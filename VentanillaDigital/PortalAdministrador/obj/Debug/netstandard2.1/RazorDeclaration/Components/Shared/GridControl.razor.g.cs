// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace PortalAdministrador.Components.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using PortalAdministrador;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using PortalAdministrador.Components.Transversales;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using PortalAdministrador.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using PortalAdministrador.Services.Parametrizacion;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using BlazorStrap;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Radzen;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using Radzen.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using PortalAdministrador.Components.Tramites;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using PortalAdministrador.Components.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\_Imports.razor"
using PortalAdministracion.Services.UsuarioAdministracion;

#line default
#line hidden
#nullable disable
    public partial class GridControl<TItem> : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 79 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
       

    [Parameter] public string Title { get; set; }
    [Parameter] public List<(string, string)> Columns { get; set; }
    [Parameter] public List<TItem> Data { get; set; }
    [Parameter]
    public long TotalRows { get; set; }
    [Parameter]
    public int TotalPages { get; set; }
    [Parameter]
    public EventCallback<int> OnChangePage { get; set; }
    [Parameter]
    public bool ShowViewAction { get; set; }
    [Parameter]
    public bool ShowEditAction { get; set; }
    [Parameter]
    public bool ShowDeleteAction { get; set; }
    [Parameter]
    public Action<TItem> OnViewItemClick { get; set; }
    [Parameter]
    public Action<TItem> OnEditItemClick { get; set; }
    [Parameter]
    public Action<TItem> OnDeleteItemClick { get; set; }
    int indicePagina;

    protected override Task OnInitializedAsync()
    {
        indicePagina = 1;
        return base.OnInitializedAsync();
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
        if (indicePagina < TotalPages)
        {
            indicePagina++;
            await OnChangePage.InvokeAsync(indicePagina);

        }
    }
    async void OnClickLast()
    {
        indicePagina = TotalPages;
        await OnChangePage.InvokeAsync(indicePagina);
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
