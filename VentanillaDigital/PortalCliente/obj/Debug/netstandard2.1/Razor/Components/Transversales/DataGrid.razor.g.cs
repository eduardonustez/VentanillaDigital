#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ea9bf85d45febc71122c912844812500e7c1317"
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
    public partial class DataGrid : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "figure");
            __builder.AddAttribute(1, "class", "fig-table");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenElement(3, "table");
            __builder.AddAttribute(4, "id", "mi-tabla");
            __builder.AddAttribute(5, "class", "tabla-tramites");
            __builder.AddMarkupContent(6, "\r\n");
#nullable restore
#line 3 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
         if (TipoTramite == "3")
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(7, "            ");
            __builder.AddMarkupContent(8, "<caption>\r\n                Haga doble-clic para ver el acta.\r\n            </caption>\r\n");
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
             if (mostrarMensajeFirmando)
            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(9, "                ");
            __builder.AddMarkupContent(10, @"<caption>
                    <p>
                    <strong>Algunas actas están siendo firmadas en este momento.</strong>
                    En breve estarán disponibles para imprimir.</p>
                    <p>Por favor, haga clic en refrescar en unos instantes.</p>
                </caption>
");
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
             
        }
        else
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(11, "            ");
            __builder.OpenElement(12, "caption");
            __builder.AddMarkupContent(13, "\r\n                ");
            __builder.AddContent(14, 
#nullable restore
#line 22 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                 textoInformativo

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(15, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(16, "\r\n");
#nullable restore
#line 24 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(17, "\r\n        ");
            __builder.OpenElement(18, "thead");
            __builder.AddMarkupContent(19, "\r\n            ");
            __builder.OpenElement(20, "tr");
            __builder.AddMarkupContent(21, "\r\n");
#nullable restore
#line 28 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                 for (int i = 0; i < columnTitles.Length; i++)
                {
                    // { "Consec.", "Tipo de Trámite", "Comparecientes"," n.º Documento",  "Fecha", "Estado" };
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                     if (i == 0 || i == 3 || i == 4)
                    {
                        

#line default
#line hidden
#nullable disable
            __builder.AddContent(22, "                        ");
            __builder.OpenElement(23, "th");
            __builder.AddAttribute(24, "class", "numerico");
            __builder.AddContent(25, 
#nullable restore
#line 34 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                              columnTitles[i]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(26, "\r\n");
#nullable restore
#line 35 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                    }
                    else if (i == 2)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(27, "                        ");
            __builder.AddMarkupContent(28, "<th class=\"numerico\">\r\n                            <span class=\"material-icons\" title=\"Número de comparecientes\" alt=\"Comparecientes\">\r\n                                people\r\n                            </span>\r\n                        </th>\r\n");
#nullable restore
#line 43 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(29, "                        ");
            __builder.OpenElement(30, "th");
            __builder.AddContent(31, 
#nullable restore
#line 46 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                             columnTitles[i]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(32, "\r\n");
#nullable restore
#line 47 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 47 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                     
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 49 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                 if (mostrarCheckboxes)
                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(33, "                    ");
            __builder.OpenElement(34, "th");
            __builder.AddMarkupContent(35, "\r\n                        ");
            __builder.OpenElement(36, "input");
            __builder.AddAttribute(37, "type", "checkbox");
            __builder.AddAttribute(38, "name", "seleccionarTodos");
            __builder.AddAttribute(39, "title", "Seleccionar todas las actas");
            __builder.AddAttribute(40, "id", "seleccionarTodosCheck");
            __builder.AddAttribute(41, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 52 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                                                                                                                 SeleccionarTodos

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(42, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(43, "\r\n");
#nullable restore
#line 54 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                }
                

#line default
#line hidden
#nullable disable
            __builder.AddContent(44, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(46, "\r\n        ");
            __builder.OpenElement(47, "tbody");
            __builder.AddMarkupContent(48, "\r\n");
#nullable restore
#line 62 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
             if (data != null)
            {
                // { "Consec.", "Tipo de Trámite", "Comparecientes"," n.º Documento",  "Fecha", "Estado" };
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 65 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                 for (int i = 0; i < data.GetLength(0); i++)
                {
                    var id = data[i, 0].ToString();

#line default
#line hidden
#nullable disable
            __builder.AddContent(49, "                    ");
            __builder.OpenElement(50, "tr");
            __builder.AddAttribute(51, "class", 
#nullable restore
#line 68 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                 data[i,5].ToString() == "Firmando" ? "tabla-tramites-pendientes" : ""

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(52, "id", 
#nullable restore
#line 68 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                                                                              id

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(53, "ondblclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 68 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                                                                                               ()=> OnSelectRow.InvokeAsync(id)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(54, "\r\n");
#nullable restore
#line 69 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                         for (int j = 0; j < data.GetLength(1); j++)
                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 71 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                             if (j == 0 || j == 2 || j == 3 || j == 4)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(55, "                                ");
            __builder.OpenElement(56, "td");
            __builder.AddAttribute(57, "class", "numerico");
            __builder.AddContent(58, 
#nullable restore
#line 74 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                      data[i, j]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(59, "\r\n");
#nullable restore
#line 75 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(60, "                                ");
            __builder.OpenElement(61, "td");
            __builder.AddContent(62, 
#nullable restore
#line 78 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                     data[i, j]

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(63, "\r\n");
#nullable restore
#line 79 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 79 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                             
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 81 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                         if (mostrarCheckboxes)
                        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(64, "                            ");
            __builder.OpenElement(65, "td");
            __builder.AddMarkupContent(66, "\r\n                                ");
            __builder.OpenElement(67, "input");
            __builder.AddAttribute(68, "class", "checkbox-table");
            __builder.AddAttribute(69, "type", "checkbox");
            __builder.AddAttribute(70, "name", "uno");
            __builder.AddAttribute(71, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 84 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                                                                   ()=> AgregarSeleccion(id)

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(72, "\r\n                            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(73, "\r\n");
#nullable restore
#line 86 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                        }
                        

#line default
#line hidden
#nullable disable
            __builder.AddContent(74, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n");
#nullable restore
#line 101 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 101 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                 
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(76, "        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(77, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(78, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(79, "\r\n");
            __builder.OpenElement(80, "div");
            __builder.AddAttribute(81, "class", "container");
            __builder.AddMarkupContent(82, "\r\n    ");
            __builder.OpenElement(83, "div");
            __builder.AddAttribute(84, "class", "paginator-container");
            __builder.AddMarkupContent(85, "\r\n        ");
            __builder.OpenElement(86, "div");
            __builder.AddAttribute(87, "class", "paginator-buttons");
            __builder.AddMarkupContent(88, "\r\n            ");
            __builder.OpenElement(89, "button");
            __builder.AddAttribute(90, "class", "btn-terciario");
            __builder.AddAttribute(91, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 109 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                    OnClickFirst

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(92, "<i class=\"fas fa-angle-double-left\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(93, "\r\n            ");
            __builder.OpenElement(94, "button");
            __builder.AddAttribute(95, "class", "btn-terciario");
            __builder.AddAttribute(96, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 110 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                    OnClickPrevious

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(97, "<i class=\"fas fa-angle-left\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(98, "\r\n            ");
            __builder.OpenElement(99, "span");
            __builder.AddContent(100, "Pagina ");
            __builder.AddContent(101, 
#nullable restore
#line 111 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                          indicePagina

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(102, " de ");
            __builder.AddContent(103, 
#nullable restore
#line 111 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                           totalPaginas

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(104, "\r\n            ");
            __builder.OpenElement(105, "button");
            __builder.AddAttribute(106, "class", "btn-terciario");
            __builder.AddAttribute(107, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 112 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                    OnClickNext

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(108, "<i class=\"fas fa-angle-right\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(109, "\r\n            ");
            __builder.OpenElement(110, "button");
            __builder.AddAttribute(111, "class", "btn-terciario");
            __builder.AddAttribute(112, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 113 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                                                    OnClickLast

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(113, "<i class=\"fas fa-angle-double-right\"></i>");
            __builder.CloseElement();
            __builder.AddMarkupContent(114, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(115, "\r\n        ");
            __builder.OpenElement(116, "p");
            __builder.AddContent(117, "Total Registros: ");
            __builder.AddContent(118, 
#nullable restore
#line 115 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
                             totalRegistros

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(119, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(120, "\r\n");
#nullable restore
#line 117 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
     if (mostrarCheckboxes)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(121, "        ");
            __builder.AddMarkupContent(122, "<a href=\"/bandejaEntrada/3\">\r\n            Ir a autorizados\r\n        </a>\r\n");
#nullable restore
#line 122 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\DataGrid.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591