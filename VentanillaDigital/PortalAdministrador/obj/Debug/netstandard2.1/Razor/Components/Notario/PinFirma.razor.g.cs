#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e924ea0337f62874655bf403545c8d9883b4a5b5"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalAdministrador.Components.Notario
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
    public partial class PinFirma : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 1 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
 if (errors)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "    ");
            __builder.AddMarkupContent(1, "<div class=\"alert mensaje-alerta\">\r\n        <p>Por favor, escriba su PIN en todos los campos.</p>\r\n    </div>\r\n");
#nullable restore
#line 6 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
}

#line default
#line hidden
#nullable disable
            __builder.OpenElement(2, "div");
            __builder.AddAttribute(3, "id", 
#nullable restore
#line 7 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
          ClaseFirma

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(4, "class", "mt-3 input-container");
            __builder.AddMarkupContent(5, "\r\n    ");
            __builder.OpenElement(6, "div");
            __builder.AddAttribute(7, "class", "d-flex justify-content-center");
            __builder.AddMarkupContent(8, "\r\n        ");
            __builder.OpenElement(9, "input");
            __builder.AddAttribute(10, "name", "pinFirma");
            __builder.AddAttribute(11, "autocomplete", "off");
            __builder.AddAttribute(12, "class", "pin-firma");
            __builder.AddAttribute(13, "type", 
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                                                           tipoDefecto

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(14, "onkeydown", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                        KeyDown

#line default
#line hidden
#nullable disable
            ));
            __builder.AddEventPreventDefaultAttribute(15, "onkeydown", 
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                             preventDefault

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(16, "placeholder", "-");
            __builder.AddAttribute(17, "id", "inputUno");
            __builder.AddAttribute(18, "maxlength", "1");
            __builder.AddAttribute(19, "required", true);
            __builder.AddAttribute(20, "autofocus", true);
            __builder.AddAttribute(21, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 9 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                               p1

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(22, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => p1 = __value, p1));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(23, "\r\n        ");
            __builder.OpenElement(24, "input");
            __builder.AddAttribute(25, "name", "pinFirma");
            __builder.AddAttribute(26, "autocomplete", "off");
            __builder.AddAttribute(27, "class", "pin-firma");
            __builder.AddAttribute(28, "type", 
#nullable restore
#line 12 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                                                           tipoDefecto

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(29, "onkeydown", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 13 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                        KeyDown

#line default
#line hidden
#nullable disable
            ));
            __builder.AddEventPreventDefaultAttribute(30, "onkeydown", 
#nullable restore
#line 13 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                             preventDefault

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(31, "placeholder", "-");
            __builder.AddAttribute(32, "maxlength", "1");
            __builder.AddAttribute(33, "required", true);
            __builder.AddAttribute(34, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 12 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                               p2

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(35, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => p2 = __value, p2));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n        ");
            __builder.OpenElement(37, "input");
            __builder.AddAttribute(38, "name", "pinFirma");
            __builder.AddAttribute(39, "autocomplete", "off");
            __builder.AddAttribute(40, "class", "pin-firma");
            __builder.AddAttribute(41, "type", 
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                                                           tipoDefecto

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(42, "onkeydown", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                        KeyDown

#line default
#line hidden
#nullable disable
            ));
            __builder.AddEventPreventDefaultAttribute(43, "onkeydown", 
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                             preventDefault

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(44, "placeholder", "-");
            __builder.AddAttribute(45, "maxlength", "1");
            __builder.AddAttribute(46, "required", true);
            __builder.AddAttribute(47, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                               p3

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(48, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => p3 = __value, p3));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n        ");
            __builder.OpenElement(50, "input");
            __builder.AddAttribute(51, "name", "pinFirma");
            __builder.AddAttribute(52, "autocomplete", "off");
            __builder.AddAttribute(53, "class", "pin-firma");
            __builder.AddAttribute(54, "type", 
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                                                           tipoDefecto

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(55, "onkeydown", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                        KeyDown

#line default
#line hidden
#nullable disable
            ));
            __builder.AddEventPreventDefaultAttribute(56, "onkeydown", 
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                             preventDefault

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(57, "placeholder", "-");
            __builder.AddAttribute(58, "maxlength", "1");
            __builder.AddAttribute(59, "required", true);
            __builder.AddAttribute(60, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                               p4

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(61, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => p4 = __value, p4));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(62, "\r\n        ");
            __builder.OpenElement(63, "div");
            __builder.AddAttribute(64, "class", "px-2 d-flex align-items-center");
            __builder.AddMarkupContent(65, "\r\n            ");
            __builder.OpenElement(66, "span");
            __builder.AddAttribute(67, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 19 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                            VerTodo

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(68, "class", "material-icons");
            __builder.AddMarkupContent(69, "\r\n                ");
            __builder.AddContent(70, 
#nullable restore
#line 20 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                 iconoVisibilidad

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(71, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(72, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(73, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(74, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n\r\n");
#nullable restore
#line 26 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
 if (ModalCambioFirma)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(76, "    ");
            __builder.OpenElement(77, "div");
            __builder.AddAttribute(78, "class", "d-flex justify-content-around mt-3");
            __builder.AddMarkupContent(79, "\r\n        ");
            __builder.OpenElement(80, "button");
            __builder.AddAttribute(81, "type", "button");
            __builder.AddAttribute(82, "class", "btn-contorno-primario");
            __builder.AddAttribute(83, "data-dismiss", "modal");
            __builder.AddAttribute(84, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 29 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                                                           LimpiarInputs

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(85, "Cancelar");
            __builder.CloseElement();
            __builder.AddMarkupContent(86, "\r\n        ");
            __builder.OpenElement(87, "button");
            __builder.AddAttribute(88, "type", "button");
            __builder.AddAttribute(89, "class", "btn-primario");
            __builder.AddAttribute(90, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 30 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                             Firmar

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(91, "\r\n            Cambiar PIN de firma electrónica\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(92, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(93, "\r\n");
#nullable restore
#line 34 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
}
else
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(94, "    ");
            __builder.OpenElement(95, "div");
            __builder.AddAttribute(96, "class", "d-flex justify-content-center mt-3");
            __builder.AddMarkupContent(97, "\r\n        ");
            __builder.OpenElement(98, "button");
            __builder.AddAttribute(99, "type", "button");
            __builder.AddAttribute(100, "class", "btn-primario");
            __builder.AddAttribute(101, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 38 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                                                             Firmar

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(102, "\r\n            ");
            __builder.OpenElement(103, "span");
            __builder.AddContent(104, 
#nullable restore
#line 39 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
                    ModalAutorizar ? "Autorizar" : "Rechazar"

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(105, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(106, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(107, "\r\n");
#nullable restore
#line 42 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\Notario\PinFirma.razor"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
