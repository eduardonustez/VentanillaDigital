#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0e331e6eab91490fcf92b4c4b9b9c0485dc79a58"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalCliente.Pages.TramitePages
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
#nullable restore
#line 1 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
using BlazorInputFile;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
           [Authorize]

#line default
#line hidden
#nullable disable
    public partial class EnviarRecaudoComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "root-initial-loader");
            __builder.AddAttribute(2, "style", "display:" + " " + (
#nullable restore
#line 4 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                                   IsLoading ? "block" : "none"

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(3, "\r\n    <div class=\"overlay\"></div>\r\n    <i class=\"fas fa-circle-notch fa-3x fa-spin-2x\"></i>\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(4, "\r\n\r\n");
            __builder.OpenElement(5, "div");
            __builder.AddAttribute(6, "class", "container");
            __builder.AddMarkupContent(7, "\r\n");
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
     if (!string.IsNullOrEmpty(MensajeError))
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(8, "        ");
            __builder.OpenElement(9, "div");
            __builder.AddAttribute(10, "class", "mensaje-container mensaje-error ml-3");
            __builder.AddMarkupContent(11, "\r\n            ");
            __builder.OpenElement(12, "p");
            __builder.AddContent(13, 
#nullable restore
#line 13 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                MensajeError

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(14, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(15, "\r\n");
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(16, "\r\n    ");
            __builder.OpenElement(17, "div");
            __builder.AddAttribute(18, "class", "row");
            __builder.AddMarkupContent(19, "\r\n        ");
            __builder.OpenElement(20, "div");
            __builder.AddAttribute(21, "class", "col-12");
            __builder.AddMarkupContent(22, "\r\n            ");
            __builder.AddMarkupContent(23, "<label for=\"txtFactura\" class=\"visually-hidden\">Cargar Archivo</label>\r\n            <br>\r\n");
#nullable restore
#line 21 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
             foreach (var file in Files)
            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(24, "                ");
            __builder.OpenElement(25, "div");
            __builder.AddAttribute(26, "class", "highlight file-selected p-2");
            __builder.AddMarkupContent(27, "\r\n                    ");
            __builder.OpenElement(28, "span");
            __builder.AddContent(29, 
#nullable restore
#line 24 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                           ShortName(file.Nombre)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n                    ");
            __builder.OpenElement(31, "span");
            __builder.AddAttribute(32, "class", "float-lg-right");
            __builder.AddAttribute(33, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 25 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                                             () => RemoveFile(file)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(34, "title", "Remover de la lista");
            __builder.AddMarkupContent(35, "\r\n                        <i class=\"fa fa-trash btn-outline-danger\"></i>\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(37, "\r\n");
#nullable restore
#line 29 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(38, "\r\n            ");
            __builder.OpenComponent<BlazorInputFile.InputFile>(39);
            __builder.AddAttribute(40, "disabled", 
#nullable restore
#line 31 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                  IsLoading

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(41, "OnChange", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<BlazorInputFile.IFileListEntry[]>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<BlazorInputFile.IFileListEntry[]>(this, 
#nullable restore
#line 31 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                                       OnFileChange

#line default
#line hidden
#nullable disable
            )));
            __builder.AddAttribute(42, "Multiple", true);
            __builder.CloseComponent();
            __builder.AddMarkupContent(43, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(44, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n\r\n");
#nullable restore
#line 44 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
     if (ArchivosSeleccionados != null && ArchivosSeleccionados.Any())
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(46, "        ");
            __builder.OpenElement(47, "div");
            __builder.AddAttribute(48, "class", "container-fluid mt-2");
            __builder.AddMarkupContent(49, "\r\n            ");
            __builder.OpenElement(50, "div");
            __builder.AddAttribute(51, "class", "row mt-2");
            __builder.AddMarkupContent(52, "\r\n                ");
            __builder.OpenElement(53, "div");
            __builder.AddAttribute(54, "class", "col-12");
            __builder.AddMarkupContent(55, "\r\n                    ");
            __builder.OpenElement(56, "figure");
            __builder.AddAttribute(57, "class", "fig-table table-responsive-sm");
            __builder.AddMarkupContent(58, "\r\n                        ");
            __builder.OpenElement(59, "table");
            __builder.AddAttribute(60, "class", "table table-hover table-sm table-striped table-bordered");
            __builder.AddMarkupContent(61, "\r\n                            ");
            __builder.AddMarkupContent(62, @"<thead>
                                <tr>
                                    <th>Archivo</th>
                                    <th class=""text-center"" style=""width: 120px;"">Seleccionar</th>
                                </tr>
                            </thead>
                            ");
            __builder.OpenElement(63, "tbody");
            __builder.AddMarkupContent(64, "\r\n");
#nullable restore
#line 58 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                 foreach (var row in ArchivosSeleccionados)
                                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(65, "                                    ");
            __builder.OpenElement(66, "tr");
            __builder.AddMarkupContent(67, "\r\n                                        ");
            __builder.OpenElement(68, "td");
            __builder.AddContent(69, 
#nullable restore
#line 61 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                             row.Archivo.Nombre

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(70, "\r\n                                        ");
            __builder.OpenElement(71, "td");
            __builder.AddAttribute(72, "class", "text-center");
            __builder.AddMarkupContent(73, "\r\n                                            ");
            __builder.OpenElement(74, "input");
            __builder.AddAttribute(75, "class", "form-check-input");
            __builder.AddAttribute(76, "type", "checkbox");
            __builder.AddAttribute(77, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 63 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                                                                                       (eventArgs) => SeleccionarArchivo(row.Archivo.ArchivosPortalVirtualId, eventArgs.Value)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(78, "id", 
#nullable restore
#line 63 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                                                                                                                                                                                     row.Archivo.ArchivosPortalVirtualId

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(79, "checked", 
#nullable restore
#line 63 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                                                                                                                                                                                                                                     row.Seleccionado

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(80, "\r\n                                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(81, "\r\n                                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(82, "\r\n");
#nullable restore
#line 66 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(83, "                            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(84, "\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(85, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(86, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(87, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(88, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(89, "\r\n");
#nullable restore
#line 73 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(90, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(91, "\r\n\r\n");
            __builder.OpenElement(92, "div");
            __builder.AddAttribute(93, "class", "d-flex justify-content-end mt-3");
            __builder.AddMarkupContent(94, "\r\n    ");
            __builder.OpenElement(95, "button");
            __builder.AddAttribute(96, "type", "button");
            __builder.AddAttribute(97, "class", "btn-primario ml-3");
            __builder.AddAttribute(98, "data-dismiss", "modal");
            __builder.AddAttribute(99, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 81 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Pages\TramitePages\EnviarRecaudoComponent.razor"
                                                                                   Enviar

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(100, "\r\n        Enviar\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(101, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591