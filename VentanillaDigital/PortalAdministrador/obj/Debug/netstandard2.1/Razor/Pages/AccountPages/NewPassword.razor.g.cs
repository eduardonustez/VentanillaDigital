#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2b25bf0bcf8ae7b6e362846a901f58116ce6964e"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalAdministrador.Pages.AccountPages
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
#nullable restore
#line 2 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\_imports.razor"
using PortalAdministrador.Services;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(LoginLayout))]
    [Microsoft.AspNetCore.Components.RouteAttribute("/NewPassword/")]
    public partial class NewPassword : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "row shadow p-3 mb-5 bg-white rounded square-form");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "col-12");
            __builder.AddMarkupContent(5, "\r\n        ");
            __builder.AddMarkupContent(6, "<h2 class=\"text-center\">Crear Contraseña</h2>\r\n        \r\n        ");
            __builder.OpenElement(7, "div");
            __builder.AddAttribute(8, "class", "modal" + " " + (
#nullable restore
#line 7 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
                           ModalClass

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(9, "data-backdrop", "static");
            __builder.AddAttribute(10, "tabindex", "-1");
            __builder.AddAttribute(11, "aria-labelledby", "modalForAtdp");
            __builder.AddAttribute(12, "aria-hidden", "true");
            __builder.AddAttribute(13, "style", "display:" + (
#nullable restore
#line 8 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
                                                                               ModalDisplay

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(14, "\r\n            ");
            __builder.OpenElement(15, "div");
            __builder.AddAttribute(16, "class", "modal-dialog modal-dialog-centered");
            __builder.AddMarkupContent(17, "\r\n                ");
            __builder.OpenElement(18, "div");
            __builder.AddAttribute(19, "class", "modal-content");
            __builder.AddMarkupContent(20, "\r\n                    ");
            __builder.AddMarkupContent(21, @"<div class=""modal-header"">
                        <h3 class=""ventanilla-subtitle"">Contraseña reestablecida</h3>
                        <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                            <span aria-hidden=""true"">&times;</span>
                        </button>
                    </div>
                    ");
            __builder.OpenElement(22, "div");
            __builder.AddAttribute(23, "class", "modal-body");
            __builder.AddMarkupContent(24, "\r\n                        ");
            __builder.AddMarkupContent(25, "<div class=\"d-flex justify-content-center mb-3\">\r\n                            <p>Actualización de contraseña Exitosa</p>\r\n                        </div>\r\n                        ");
            __builder.OpenElement(26, "div");
            __builder.AddAttribute(27, "class", "d-flex justify-content-around");
            __builder.AddMarkupContent(28, "\r\n                            ");
            __builder.OpenElement(29, "button");
            __builder.AddAttribute(30, "type", "button");
            __builder.AddAttribute(31, "class", "btn-primario");
            __builder.AddAttribute(32, "data-dismiss", "modal");
            __builder.AddAttribute(33, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 22 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
                                                                                                      () => Close()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(34, "Aceptar");
            __builder.CloseElement();
            __builder.AddMarkupContent(35, "\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(37, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(38, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(39, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(40, "\r\n");
#nullable restore
#line 28 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
         if (ShowBackdrop)
        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(41, "            <div class=\"modal-backdrop fade show\"></div>\r\n");
#nullable restore
#line 31 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(42, "        \r\n\r\n    ");
            __builder.OpenElement(43, "form");
            __builder.AddAttribute(44, "onsubmit", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.EventArgs>(this, 
#nullable restore
#line 34 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
                     HandleCambiarContrasena

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(45, "\r\n        ");
            __builder.AddMarkupContent(46, @"<div class=""password-rules"">
            <h6>Las contraseñas deben tener al menos 8 caracteres y contener al menos un carácter de cada tipo: </h6>
            <ul>
                <li>Carácteres en mayúscula (A-Z)</li>
                <li>Carácteres en minúscula (a-z)</li>
                <li>Números (0-9)</li>
                <li>Carácteres especiales (e.j. !@#$%,^&*)</li>
            </ul>
        </div>
        ");
            __builder.AddMarkupContent(47, "<strong>Contraseña</strong>\r\n        ");
            __builder.OpenElement(48, "input");
            __builder.AddAttribute(49, "type", "password");
            __builder.AddAttribute(50, "class", "form-control bg-col mb-3");
            __builder.AddAttribute(51, "placeholder", "******");
            __builder.AddAttribute(52, "required", true);
            __builder.AddAttribute(53, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 45 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
                                                                        pass

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(54, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => pass = __value, pass));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(55, "\r\n        ");
            __builder.AddMarkupContent(56, "<strong>Confirma tu Contraseña</strong>\r\n        ");
            __builder.OpenElement(57, "input");
            __builder.AddAttribute(58, "type", "password");
            __builder.AddAttribute(59, "class", "form-control bg-col");
            __builder.AddAttribute(60, "placeholder", "******");
            __builder.AddAttribute(61, "required", true);
            __builder.AddAttribute(62, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 47 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
                                                                   confirpass

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(63, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => confirpass = __value, confirpass));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(64, "\r\n");
#nullable restore
#line 48 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
         if (banFail)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(65, "            ");
            __builder.OpenElement(66, "div");
            __builder.AddAttribute(67, "class", "mensaje-container mensaje-error");
            __builder.AddMarkupContent(68, "\r\n                ");
            __builder.OpenElement(69, "p");
            __builder.AddContent(70, 
#nullable restore
#line 51 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
                    NewPasswordMessage

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(71, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(72, "\r\n");
#nullable restore
#line 53 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Pages\AccountPages\NewPassword.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(73, "        ");
            __builder.AddMarkupContent(74, "<div class=\"text-center mt-2\">\r\n            <button type=\"submit\" class=\"btn-primario\">Aceptar</button>\r\n        </div>\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n        ");
            __builder.AddMarkupContent(76, "<div class=\"text-center mt-2\">\r\n            <span>Desarrollado por</span><img class=\"logo-olimpia ml-1\" src=\"./images/logo-olimpia-color.svg\">\r\n        </div>\r\n\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(77, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591