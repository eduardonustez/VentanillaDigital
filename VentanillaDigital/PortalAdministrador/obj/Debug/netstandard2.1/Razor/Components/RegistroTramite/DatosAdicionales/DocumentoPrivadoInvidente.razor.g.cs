#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\DatosAdicionales\DocumentoPrivadoInvidente.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d5ac9e73f7f1afe4b3e31b1e4d204365cad273b1"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalAdministrador.Components.RegistroTramite.DatosAdicionales
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
    public partial class DocumentoPrivadoInvidente : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "form");
            __builder.AddAttribute(1, "Model", "documentoPrivado");
            __builder.AddAttribute(2, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 1 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\DatosAdicionales\DocumentoPrivadoInvidente.razor"
                                         oninput

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(3, "\r\n    ");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "input-container");
            __builder.AddMarkupContent(6, "\r\n        ");
            __builder.AddMarkupContent(7, "<label>Acto</label>\r\n        ");
            __builder.OpenElement(8, "textarea");
            __builder.AddAttribute(9, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 4 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\DatosAdicionales\DocumentoPrivadoInvidente.razor"
                               documentoPrivado.ActoNotarial

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(10, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => documentoPrivado.ActoNotarial = __value, documentoPrivado.ActoNotarial));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(11, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(12, "\r\n    ");
            __builder.OpenElement(13, "div");
            __builder.AddAttribute(14, "class", "input-container");
            __builder.AddMarkupContent(15, "\r\n        ");
            __builder.AddMarkupContent(16, "<label>Firmantes</label>\r\n        ");
            __builder.OpenElement(17, "textarea");
            __builder.AddAttribute(18, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 8 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\DatosAdicionales\DocumentoPrivadoInvidente.razor"
                               documentoPrivado.Firmantes

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(19, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => documentoPrivado.Firmantes = __value, documentoPrivado.Firmantes));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(20, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(21, "\r\n    ");
            __builder.OpenElement(22, "div");
            __builder.AddAttribute(23, "class", "input-container");
            __builder.AddMarkupContent(24, "\r\n        ");
            __builder.AddMarkupContent(25, "<label>Descripción</label>\r\n        ");
            __builder.OpenElement(26, "textarea");
            __builder.AddAttribute(27, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 12 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\DatosAdicionales\DocumentoPrivadoInvidente.razor"
                               documentoPrivado.DetalleDeActo

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(28, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => documentoPrivado.DetalleDeActo = __value, documentoPrivado.DetalleDeActo));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n    ");
            __builder.OpenElement(31, "label");
            __builder.AddAttribute(32, "class", "check-box-container");
            __builder.AddMarkupContent(33, "\r\n        No sabe firmar\r\n        ");
            __builder.OpenElement(34, "input");
            __builder.AddAttribute(35, "type", "checkbox");
            __builder.AddAttribute(36, "name", "diligenciaPersonaInvidente");
            __builder.AddAttribute(37, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\DatosAdicionales\DocumentoPrivadoInvidente.razor"
                            NoSabeFirmar

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(38, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => NoSabeFirmar = __value, NoSabeFirmar));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(39, "\r\n        <span class=\"check\" for=\"diligenciaPersonaInvidente\"></span>\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(40, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
