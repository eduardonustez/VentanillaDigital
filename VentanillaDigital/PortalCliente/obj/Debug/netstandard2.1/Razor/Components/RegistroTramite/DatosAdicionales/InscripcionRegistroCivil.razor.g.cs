#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7523268e946a4b660c2f484c78c737c32cc20927"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalCliente.Components.RegistroTramite.DatosAdicionales
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
    public partial class InscripcionRegistroCivil : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "form");
            __builder.AddAttribute(1, "Model", "inscripcionRegCivil");
            __builder.AddAttribute(2, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 1 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                                            oninput

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(3, "\r\n    ");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "input-container");
            __builder.AddMarkupContent(6, "\r\n        ");
            __builder.AddMarkupContent(7, "<label>Tipo de registro civil *</label>\r\n            ");
            __builder.OpenElement(8, "select");
            __builder.AddAttribute(9, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                               onTipoRegistroSelected

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(10, "required", true);
            __builder.AddMarkupContent(11, "\r\n            ");
            __builder.OpenElement(12, "option");
            __builder.AddAttribute(13, "disabled", true);
            __builder.AddAttribute(14, "selected", true);
            __builder.AddContent(15, "--Seleccione--");
            __builder.CloseElement();
            __builder.AddMarkupContent(16, "\r\n");
#nullable restore
#line 12 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
              if (TipoRegistroCivil != null)
            {
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                 foreach (var cnt in TipoRegistroCivil)
                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(17, "                    ");
            __builder.OpenElement(18, "option");
            __builder.AddAttribute(19, "value", 
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                                    cnt

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(20, 
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                                          cnt

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(21, "\r\n");
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                 
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(22, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(23, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(24, "\r\n    ");
            __builder.OpenElement(25, "div");
            __builder.AddAttribute(26, "class", "input-container");
            __builder.AddMarkupContent(27, "\r\n        ");
            __builder.AddMarkupContent(28, "<label>Nombres y apellidos inscritos *</label>\r\n        ");
            __builder.OpenElement(29, "textarea");
            __builder.AddAttribute(30, "required", true);
            __builder.AddAttribute(31, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 32 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\DatosAdicionales\InscripcionRegistroCivil.razor"
                               inscripcionRegCivil.NombresApellidosInscritos

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(32, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => inscripcionRegCivil.NombresApellidosInscritos = __value, inscripcionRegCivil.NombresApellidosInscritos));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(33, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(34, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
