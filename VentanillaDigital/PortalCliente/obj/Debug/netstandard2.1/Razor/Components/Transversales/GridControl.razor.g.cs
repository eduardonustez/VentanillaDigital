#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0d827687ebcde7f3064e90f50dc73b7817e25c22"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalCliente.Components.Transversales
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using PortalCliente;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using PortalCliente.Components.Transversales;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using PortalCliente.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using PortalCliente.Services.Parametrizacion;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using BlazorStrap;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Radzen;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Radzen.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Blazor.Analytics;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Blazor.Analytics.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\_Imports.razor"
using Blazored.Typeahead;

#line default
#line hidden
#nullable disable
    public partial class GridControl<TItem> : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "figure");
            __builder.AddAttribute(1, "class", "fig-table table-responsive");
            __builder.AddMarkupContent(2, "\r\n\r\n    ");
            __builder.OpenElement(3, "table");
            __builder.AddAttribute(4, "class", "table table-hover table-striped tabla-tramites-virtuales");
            __builder.AddMarkupContent(5, "\r\n");
#nullable restore
#line 4 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
         if (!string.IsNullOrEmpty(Title))
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(6, "            ");
            __builder.OpenElement(7, "caption");
            __builder.AddMarkupContent(8, "\r\n                ");
            __builder.AddContent(9, 
#nullable restore
#line 7 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                 Title

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(10, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(11, "\r\n");
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(12, "\r\n");
#nullable restore
#line 11 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
         if (Columns != null && Columns.Count > 0)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(13, "            ");
            __builder.OpenElement(14, "thead");
            __builder.AddMarkupContent(15, "\r\n                ");
            __builder.OpenElement(16, "tr");
            __builder.AddMarkupContent(17, "\r\n");
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                     foreach (var col in Columns)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(18, "                        ");
            __builder.OpenElement(19, "th");
            __builder.AddContent(20, 
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                             col.Item1

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(21, "\r\n");
#nullable restore
#line 18 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                    }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(22, "\r\n");
#nullable restore
#line 20 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                     if (ShowViewAction || ShowEditAction || ShowDeleteAction)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(23, "                        ");
            __builder.AddMarkupContent(24, "<th>Acciones</th>\r\n");
#nullable restore
#line 23 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                    }

#line default
#line hidden
#nullable disable
            __builder.AddContent(25, "                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(26, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(27, "\r\n            ");
            __builder.OpenElement(28, "tbody");
            __builder.AddMarkupContent(29, "\r\n");
#nullable restore
#line 27 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                 if (Data != null)
                {
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 29 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                     foreach (var row in Data)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(30, "                        ");
            __builder.OpenElement(31, "tr");
            __builder.AddMarkupContent(32, "\r\n");
#nullable restore
#line 32 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                             foreach (var col in Columns)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(33, "                                ");
            __builder.OpenElement(34, "td");
            __builder.AddContent(35, 
#nullable restore
#line 34 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                     row.GetType().GetProperty(col.Item2).GetValue(row, null)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n");
#nullable restore
#line 35 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                            }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(37, "\r\n");
#nullable restore
#line 37 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                             if (ShowViewAction || ShowEditAction || ShowDeleteAction)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(38, "                                ");
            __builder.OpenElement(39, "td");
            __builder.AddMarkupContent(40, "\r\n");
#nullable restore
#line 40 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                     if (ShowViewAction)
                                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(41, "                                        ");
            __builder.OpenElement(42, "button");
            __builder.AddAttribute(43, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 42 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                                            () => OnViewItemClick(row)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(44, "<i class=\"fa fa-eye\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n");
#nullable restore
#line 43 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                    }

#line default
#line hidden
#nullable disable
            __builder.AddContent(46, "                                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(47, "\r\n");
#nullable restore
#line 45 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(48, "                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n");
#nullable restore
#line 47 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 47 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                     
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(50, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(51, "\r\n");
#nullable restore
#line 50 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(52, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(53, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(54, "\r\n    ");
            __builder.OpenElement(55, "div");
            __builder.AddAttribute(56, "class", "paginator-container");
            __builder.AddMarkupContent(57, "\r\n        ");
            __builder.OpenElement(58, "div");
            __builder.AddAttribute(59, "class", "paginator-buttons");
            __builder.AddMarkupContent(60, "\r\n            ");
            __builder.OpenElement(61, "button");
            __builder.AddAttribute(62, "class", "btn-terciario");
            __builder.AddAttribute(63, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 55 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                                    OnClickFirst

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(64, "<i class=\"fas fa-angle-double-left\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(65, "\r\n            ");
            __builder.OpenElement(66, "button");
            __builder.AddAttribute(67, "class", "btn-terciario");
            __builder.AddAttribute(68, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 56 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                                    OnClickPrevious

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(69, "<i class=\"fas fa-angle-left\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(70, "\r\n            ");
            __builder.OpenElement(71, "span");
            __builder.AddContent(72, "Pagina ");
            __builder.AddContent(73, 
#nullable restore
#line 57 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                          indicePagina

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(74, " de ");
            __builder.AddContent(75, 
#nullable restore
#line 57 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                           TotalPages

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(76, "\r\n            ");
            __builder.OpenElement(77, "button");
            __builder.AddAttribute(78, "class", "btn-terciario");
            __builder.AddAttribute(79, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 58 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                                    OnClickNext

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(80, "<i class=\"fas fa-angle-right\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(81, "\r\n            ");
            __builder.OpenElement(82, "button");
            __builder.AddAttribute(83, "class", "btn-terciario");
            __builder.AddAttribute(84, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 59 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                                                    OnClickLast

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(85, "<i class=\"fas fa-angle-double-right\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(86, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(87, "\r\n        ");
            __builder.OpenElement(88, "p");
            __builder.AddContent(89, "Total Registros: ");
            __builder.AddContent(90, 
#nullable restore
#line 61 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
                             TotalRows

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(91, "\r\n    ");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 66 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\GridControl.razor"
       

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
