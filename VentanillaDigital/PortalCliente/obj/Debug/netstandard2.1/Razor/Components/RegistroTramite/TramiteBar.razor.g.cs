#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "56af06f727a459562d814ef3f69a952f28bf86bd"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalCliente.Components.RegistroTramite
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
    public partial class TramiteBar : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "cabecera-tramite-container desktop");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "cabecera-tramite-content");
            __builder.AddMarkupContent(5, "\r\n");
#nullable restore
#line 3 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
         if (TramiteId == 0)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(6, "            ");
            __builder.AddMarkupContent(7, "<p class=\"pl-1\">Nuevo tr??mite</p>\r\n");
#nullable restore
#line 6 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
        }
        else
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(8, "            ");
            __builder.OpenElement(9, "p");
            __builder.AddContent(10, "Tramite # ");
            __builder.OpenElement(11, "span");
            __builder.AddContent(12, 
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                TramiteId

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(13, "\r\n");
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
         if (TipoTramite != null)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(14, "            ");
            __builder.OpenElement(15, "p");
            __builder.AddContent(16, 
#nullable restore
#line 13 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                TipoTramite.Nombre

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n            ");
            __builder.OpenElement(18, "p");
            __builder.AddMarkupContent(19, "\r\n                Comparecientes ");
            __builder.OpenElement(20, "span");
            __builder.AddContent(21, 
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                      ComparecienteActual

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(22, " de\r\n                ");
            __builder.OpenElement(23, "span");
            __builder.AddContent(24, 
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                       Comparecientes

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddContent(25, " ");
            __builder.AddContent(26, 
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                              ComparecienteFirmaRuego

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(27, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(28, "\r\n");
#nullable restore
#line 18 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(29, "\r\n        ");
            __builder.AddMarkupContent(30, "<button class=\"cancelar-desk\" data-toggle=\"modal\" data-target=\"#cancelarTramiteModal\">\r\n            <i class=\"material-icons mr-1 md-18\">cancel</i> Cancelar tr??mite\r\n        </button>\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(31, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(32, "\r\n\r\n");
            __builder.OpenElement(33, "div");
            __builder.AddAttribute(34, "class", "cabecera-tramite-container mobile");
            __builder.AddMarkupContent(35, "\r\n    ");
            __builder.OpenElement(36, "div");
            __builder.AddAttribute(37, "class", "cabecera-tramite-content mobile");
            __builder.AddMarkupContent(38, "\r\n    ");
            __builder.OpenElement(39, "div");
            __builder.AddAttribute(40, "class", "datos-tramite");
            __builder.AddMarkupContent(41, "\r\n");
#nullable restore
#line 29 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
         if (TramiteId == 0)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(42, "            ");
            __builder.AddMarkupContent(43, "<p class=\"pl-1\">Nuevo tr??mite</p>\r\n");
#nullable restore
#line 32 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
        }
        else
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(44, "            ");
            __builder.OpenElement(45, "p");
            __builder.AddContent(46, "Tramite # ");
            __builder.OpenElement(47, "span");
            __builder.AddContent(48, 
#nullable restore
#line 35 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                TramiteId

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n");
#nullable restore
#line 36 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
         if (TipoTramite != null)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(50, "            ");
            __builder.OpenElement(51, "p");
            __builder.AddContent(52, 
#nullable restore
#line 39 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                TipoTramite.Nombre

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(53, "\r\n            ");
            __builder.OpenElement(54, "p");
            __builder.AddMarkupContent(55, "\r\n                Comparecientes ");
            __builder.OpenElement(56, "span");
            __builder.AddContent(57, 
#nullable restore
#line 41 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                      ComparecienteActual

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(58, " de\r\n                ");
            __builder.OpenElement(59, "span");
            __builder.AddContent(60, 
#nullable restore
#line 42 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                       Comparecientes

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddContent(61, " ");
            __builder.AddContent(62, 
#nullable restore
#line 42 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                              ComparecienteFirmaRuego

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(63, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(64, "\r\n");
#nullable restore
#line 44 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(65, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(66, "\r\n          ");
            __builder.OpenElement(67, "button");
            __builder.AddAttribute(68, "class", "cancelar-mobile");
            __builder.AddAttribute(69, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 46 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                                    (e=>cancelarTramiteModal.Open())

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(70, "\r\n            ");
            __builder.AddMarkupContent(71, "<i class=\"material-icons mr-1 mb-1 md-18\">cancel</i> Cancelar tr??mite\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(72, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(73, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(74, "\r\n\r\n\r\n");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.CancelarTramiteModal>(75);
            __builder.AddAttribute(76, "MotivoCancelacionChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 54 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                          CancelarTramite

#line default
#line hidden
#nullable disable
            )));
            __builder.AddComponentReferenceCapture(77, (__value) => {
#nullable restore
#line 53 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\TramiteBar.razor"
                                                                     cancelarTramiteModal = (PortalCliente.Components.RegistroTramite.CancelarTramiteModal)__value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
