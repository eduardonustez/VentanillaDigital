#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a151bc6a377a2205e2de08df0506c177ceff37cd"
// <auto-generated/>
#pragma warning disable 1591
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
            __builder.OpenElement(0, "figure");
            __builder.AddAttribute(1, "class", "fig-table table-responsive");
            __builder.AddMarkupContent(2, "\r\n\r\n    ");
            __builder.OpenElement(3, "table");
            __builder.AddAttribute(4, "class", "table table-hover table-striped");
            __builder.AddMarkupContent(5, "\r\n");
#nullable restore
#line 4 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
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
#line 7 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                 Title

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(10, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(11, "\r\n");
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(12, "\r\n");
#nullable restore
#line 11 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
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
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                     foreach (var col in Columns)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(18, "                        ");
            __builder.OpenElement(19, "th");
            __builder.AddContent(20, 
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                             col.Item1

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(21, "\r\n");
#nullable restore
#line 18 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                    }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(22, "\r\n");
#nullable restore
#line 20 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                     if (ShowViewAction || ShowEditAction || ShowDeleteAction)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(23, "                        ");
            __builder.AddMarkupContent(24, "<th>Acciones</th>\r\n");
#nullable restore
#line 23 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
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
#line 27 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                 if (Data != null)
                {
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 29 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                     foreach (var row in Data)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(30, "                        ");
            __builder.OpenElement(31, "tr");
            __builder.AddMarkupContent(32, "\r\n");
#nullable restore
#line 32 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                             foreach (var col in Columns)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(33, "                                ");
            __builder.OpenElement(34, "td");
            __builder.AddContent(35, 
#nullable restore
#line 34 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                     row.GetType().GetProperty(col.Item2).GetValue(row, null)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n");
#nullable restore
#line 35 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                            }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(37, "\r\n");
#nullable restore
#line 37 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                             if (ShowViewAction || ShowEditAction || ShowDeleteAction)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(38, "                        ");
            __builder.OpenElement(39, "td");
            __builder.AddMarkupContent(40, "\r\n                            ");
            __builder.OpenElement(41, "div");
            __builder.AddAttribute(42, "class", "d-flex");
            __builder.AddMarkupContent(43, "\r\n");
#nullable restore
#line 41 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                 if (ShowViewAction)
                                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(44, "                                    ");
            __builder.OpenElement(45, "button");
            __builder.AddAttribute(46, "class", "btn-contorno-primario mr-2");
            __builder.AddAttribute(47, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 43 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                                                           () => OnViewItemClick(row)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(48, "<i class=\"fa fa-eye mr-2\"></i> Ver");
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n");
#nullable restore
#line 44 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 45 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                 if (ShowEditAction)
                                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(50, "                                    ");
            __builder.OpenElement(51, "button");
            __builder.AddAttribute(52, "class", "btn-contorno-primario mr-2");
            __builder.AddAttribute(53, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 47 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                                                           () => OnEditItemClick(row)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(54, "<i class=\"fa fa-edit\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(55, "\r\n");
#nullable restore
#line 48 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 49 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                 if (ShowDeleteAction)
                                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(56, "                                    ");
            __builder.OpenElement(57, "button");
            __builder.AddAttribute(58, "class", "btn-primario");
            __builder.AddAttribute(59, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 51 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                                             () => OnDeleteItemClick(row)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(60, "<i class=\"fa fa-trash\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(61, "\r\n");
#nullable restore
#line 52 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(62, "                            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(63, "\r\n\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(64, "\r\n");
#nullable restore
#line 56 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(65, "                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(66, "\r\n");
#nullable restore
#line 58 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 58 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                     
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(67, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(68, "\r\n");
#nullable restore
#line 61 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(69, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(70, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(71, "\r\n");
            __builder.OpenElement(72, "div");
            __builder.AddAttribute(73, "class", "container");
            __builder.AddMarkupContent(74, "\r\n    ");
            __builder.OpenElement(75, "div");
            __builder.AddAttribute(76, "class", "paginator-container");
            __builder.AddMarkupContent(77, "\r\n        ");
            __builder.OpenElement(78, "div");
            __builder.AddAttribute(79, "class", "paginator-buttons");
            __builder.AddMarkupContent(80, "\r\n            ");
            __builder.OpenElement(81, "button");
            __builder.AddAttribute(82, "class", "btn-terciario");
            __builder.AddAttribute(83, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 67 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                    OnClickFirst

#line default
#line hidden
#nullable disable
            ));
            __builder.OpenElement(84, "i");
            __builder.AddAttribute(85, "class", "fas fa-angle-double-left");
            __builder.AddAttribute(86, "disabled", 
#nullable restore
#line 67 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                                                                                  indicePagina == 1

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(87, "\r\n            ");
            __builder.OpenElement(88, "button");
            __builder.AddAttribute(89, "class", "btn-terciario");
            __builder.AddAttribute(90, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 68 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                    OnClickPrevious

#line default
#line hidden
#nullable disable
            ));
            __builder.OpenElement(91, "i");
            __builder.AddAttribute(92, "class", "fas fa-angle-left");
            __builder.AddAttribute(93, "disabled", 
#nullable restore
#line 68 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                                                                              indicePagina == 1

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(94, "\r\n            ");
            __builder.OpenElement(95, "span");
            __builder.AddMarkupContent(96, "P??gina ");
            __builder.AddContent(97, 
#nullable restore
#line 69 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                          indicePagina

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(98, " de ");
            __builder.AddContent(99, 
#nullable restore
#line 69 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                           TotalPages

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(100, "\r\n            ");
            __builder.OpenElement(101, "button");
            __builder.AddAttribute(102, "class", "btn-terciario");
            __builder.AddAttribute(103, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 70 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                    OnClickNext

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(104, "disabled", 
#nullable restore
#line 70 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                                             indicePagina == TotalPages

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(105, "<i class=\"fas fa-angle-right\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(106, "\r\n            ");
            __builder.OpenElement(107, "button");
            __builder.AddAttribute(108, "class", "btn-terciario");
            __builder.AddAttribute(109, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 71 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                    OnClickLast

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(110, "disabled", 
#nullable restore
#line 71 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                                                                             indicePagina == TotalPages

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(111, "<i class=\"fas fa-angle-double-right\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(112, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(113, "\r\n        ");
            __builder.OpenElement(114, "p");
            __builder.AddContent(115, "Total Registros: ");
            __builder.AddContent(116, 
#nullable restore
#line 73 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Shared\GridControl.razor"
                             TotalRows

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(117, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(118, "\r\n");
            __builder.CloseElement();
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
