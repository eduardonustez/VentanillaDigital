#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "885d42fc4bca97c1d203f22ce01e2f67520f87ab"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalAdministrador.Components.RegistroTramite
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
    public partial class CapturaHuella : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "col-12");
            __builder.AddMarkupContent(2, "\r\n    ");
            __builder.AddMarkupContent(3, "<h3>Captura de huellas</h3>\r\n    ");
            __builder.OpenElement(4, "label");
            __builder.AddAttribute(5, "class", "check-box-container tramite-sin-biometria");
            __builder.AddMarkupContent(6, "\r\n        Tr??mite sin biometr??a\r\n        ");
            __builder.OpenElement(7, "input");
            __builder.AddAttribute(8, "type", "checkbox");
            __builder.AddAttribute(9, "name", "tramiteSinBiometria");
            __builder.AddAttribute(10, "disabled", 
#nullable restore
#line 30 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                     _chkSinBiometriaIsDisabled

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(11, "checked", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 31 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                   Compareciente.TramiteSinBiometria

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Compareciente.TramiteSinBiometria = __value, Compareciente.TramiteSinBiometria));
            __builder.SetUpdatesAttributeName("checked");
            __builder.CloseElement();
            __builder.AddMarkupContent(13, "\r\n        <span class=\"check\" for=\"tramiteSinBiometria\"></span>\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(14, "\r\n");
#nullable restore
#line 34 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
     if (Compareciente.TramiteSinBiometria)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(15, "        ");
            __builder.AddMarkupContent(16, "<p>Ingrese el motivo por el cual el ciudadano no realiza la validaci??n bi??metrica</p>\r\n        ");
            __builder.OpenElement(17, "form");
            __builder.AddMarkupContent(18, "\r\n            ");
            __builder.OpenElement(19, "div");
            __builder.AddAttribute(20, "class", "input-container");
            __builder.AddMarkupContent(21, "\r\n                ");
            __builder.AddMarkupContent(22, "<label>Seleccione el motivo</label>\r\n                ");
            __builder.OpenElement(23, "select");
            __builder.AddAttribute(24, "disabled", 
#nullable restore
#line 40 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                   _slSinBiometriaIsDisabled

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(25, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 40 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                         async (v)=> { _motivo = int.Parse(v.Value.ToString());
                                   await ReadyChanged.InvokeAsync(_motivo > 0); }

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(26, "\r\n                    ");
            __builder.OpenElement(27, "option");
            __builder.AddAttribute(28, "disabled", true);
            __builder.AddContent(29, "Seleccione");
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n");
#nullable restore
#line 43 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                     foreach (KeyValuePair<int, string> entry in MotivosSinBiometria)
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(31, "                        ");
            __builder.OpenElement(32, "option");
            __builder.AddAttribute(33, "value", 
#nullable restore
#line 45 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                        entry.Key

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(34, "selected", 
#nullable restore
#line 45 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                               entry.Key == _motivo

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(35, 
#nullable restore
#line 45 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                                       entry.Value

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n");
#nullable restore
#line 46 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                    }

#line default
#line hidden
#nullable disable
            __builder.AddContent(37, "                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(38, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(39, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(40, "\r\n");
#nullable restore
#line 56 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
    }
    else
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(41, "        ");
            __builder.AddMarkupContent(42, "<p class=\"mt-2 mb-2\">Haga clic sobre la imagen para iniciar la captura de la huella</p>\r\n        ");
            __builder.OpenElement(43, "div");
            __builder.AddAttribute(44, "class", "d-flex col-12 captura-huellas");
            __builder.AddMarkupContent(45, "\r\n            ");
            __builder.OpenElement(46, "div");
            __builder.AddAttribute(47, "class", "mr-4 dedo-solicitado-container");
            __builder.AddMarkupContent(48, "\r\n                ");
            __builder.OpenElement(49, "div");
            __builder.AddAttribute(50, "class", "img-dedo-container" + " " + (
#nullable restore
#line 62 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                 _dedo1Class

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(51, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 62 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                         Captura1

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(52, "\r\n                    ");
            __builder.OpenElement(53, "img");
            __builder.AddAttribute(54, "src", 
#nullable restore
#line 63 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                               _imgDedoSolicitado1

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(55, "class", "img-dedo");
            __builder.CloseElement();
            __builder.AddMarkupContent(56, "\r\n                    ");
            __builder.OpenElement(57, "p");
            __builder.AddAttribute(58, "class", "mt-2 mb-2");
            __builder.AddContent(59, 
#nullable restore
#line 64 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                          _lblDedoSolicitado1

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(60, " ");
            __builder.CloseElement();
            __builder.AddMarkupContent(61, "\r\n                    ");
            __builder.OpenElement(62, "div");
            __builder.AddAttribute(63, "class", "estado-captura-container" + " " + (
#nullable restore
#line 65 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                           _dedo1Class

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(64, "\r\n");
#nullable restore
#line 66 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                         if (_capturandoHuella1)
                        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(65, "                            <div class=\"estado-captura-huella spinner\"></div>\r\n");
#nullable restore
#line 69 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(66, "                            <span class=\"estado-captura-huella \"></span>\r\n");
#nullable restore
#line 73 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(67, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(68, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(69, "\r\n                ");
            __builder.OpenElement(70, "div");
            __builder.AddAttribute(71, "class", "progress");
            __builder.AddMarkupContent(72, "\r\n                    ");
            __builder.OpenElement(73, "div");
            __builder.AddAttribute(74, "class", "progress-bar" + " " + (
#nullable restore
#line 77 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                               _claseCalidadHuella1

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(75, "role", "progressbar");
            __builder.AddAttribute(76, "style", "width:" + " " + (
#nullable restore
#line 77 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                                                         $"{_calidadHuella1}%"

#line default
#line hidden
#nullable disable
            ) + ";");
            __builder.AddAttribute(77, "aria-valuenow", 
#nullable restore
#line 77 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                                                                                                  _calidadHuella1

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(78, "aria-valuemin", "0");
            __builder.AddAttribute(79, "aria-valuemax", "100");
            __builder.AddContent(80, 
#nullable restore
#line 78 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                 $"{_calidadHuella1}%"

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(81, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(82, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(83, "\r\n            ");
            __builder.OpenElement(84, "div");
            __builder.AddAttribute(85, "class", "mr-4 dedo-solicitado-container");
            __builder.AddMarkupContent(86, "\r\n                ");
            __builder.OpenElement(87, "div");
            __builder.AddAttribute(88, "class", "img-dedo-container" + " " + (
#nullable restore
#line 82 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                 _dedo2Class

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(89, "\r\n                    ");
            __builder.OpenElement(90, "img");
            __builder.AddAttribute(91, "src", 
#nullable restore
#line 83 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                               _imgDedoSolicitado2

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(92, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 83 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                              Captura2

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(93, "class", "img-dedo");
            __builder.CloseElement();
            __builder.AddMarkupContent(94, "\r\n                    ");
            __builder.OpenElement(95, "p");
            __builder.AddAttribute(96, "class", "mt-2 mb-2");
            __builder.AddContent(97, 
#nullable restore
#line 84 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                          _lblDedoSolicitado2

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(98, " ");
            __builder.CloseElement();
            __builder.AddMarkupContent(99, "\r\n                    ");
            __builder.OpenElement(100, "div");
            __builder.AddAttribute(101, "class", "estado-captura-container" + "  " + (
#nullable restore
#line 85 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                            _dedo2Class

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(102, "\r\n");
#nullable restore
#line 86 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                         if (_capturandoHuella2)
                        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(103, "                            <div class=\"estado-captura-huella spinner\"></div>\r\n");
#nullable restore
#line 89 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(104, "                            <span class=\"estado-captura-huella \"></span>\r\n");
#nullable restore
#line 93 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(105, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(106, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(107, "\r\n                ");
            __builder.OpenElement(108, "div");
            __builder.AddAttribute(109, "class", "progress");
            __builder.AddMarkupContent(110, "\r\n                    ");
            __builder.OpenElement(111, "div");
            __builder.AddAttribute(112, "class", "progress-bar" + " " + (
#nullable restore
#line 97 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                               _claseCalidadHuella2

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(113, "role", "progressbar");
            __builder.AddAttribute(114, "style", "width:" + " " + (
#nullable restore
#line 97 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                                                         $"{_calidadHuella2}%"

#line default
#line hidden
#nullable disable
            ) + ";");
            __builder.AddAttribute(115, "aria-valuenow", 
#nullable restore
#line 97 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                                                                                                  _calidadHuella2

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(116, "aria-valuemin", "0");
            __builder.AddAttribute(117, "aria-valuemax", "100");
            __builder.AddContent(118, 
#nullable restore
#line 98 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                               $"{_calidadHuella2}%"

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(119, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(120, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(121, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(122, "\r\n        ");
            __builder.AddMarkupContent(123, "<div class=\"mt-2\">\r\n            <a data-toggle=\"modal\" data-target=\"#modalForExcepciones\" class=\"link-primario mt-2 mb-2\" href>\r\n                Haga clic aqu?? si alguna de las huellas solicitadas no puede ser capturada\r\n            </a>\r\n        </div>\r\n");
            __builder.AddContent(124, "        ");
            __builder.OpenElement(125, "div");
            __builder.AddAttribute(126, "class", "modal fade");
            __builder.AddAttribute(127, "id", "modalForExcepciones");
            __builder.AddAttribute(128, "data-backdrop", "static");
            __builder.AddAttribute(129, "data-keyboard", "false");
            __builder.AddAttribute(130, "tabindex", "-1");
            __builder.AddAttribute(131, "aria-labelledby", "modalForAtdp");
            __builder.AddAttribute(132, "aria-hidden", "true");
            __builder.AddMarkupContent(133, "\r\n            ");
            __builder.OpenElement(134, "div");
            __builder.AddAttribute(135, "class", "modal-dialog modal-dialog-centered");
            __builder.AddMarkupContent(136, "\r\n                ");
            __builder.OpenElement(137, "div");
            __builder.AddAttribute(138, "class", "modal-content");
            __builder.AddMarkupContent(139, "\r\n                    ");
            __builder.AddMarkupContent(140, @"<div class=""modal-header"">
                        <h3>Excepci??n de huella</h3>
                        <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                            <span aria-hidden=""true"">&times;</span>
                        </button>
                    </div>
                    ");
            __builder.OpenElement(141, "div");
            __builder.AddAttribute(142, "class", "modal-body");
            __builder.AddMarkupContent(143, "\r\n                        ");
            __builder.OpenComponent<PortalAdministrador.Components.RegistroTramite.ExcepcionHuella>(144);
            __builder.AddAttribute(145, "Excepciones", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.List<PortalAdministrador.Services.Biometria.Models.Dedo>>(
#nullable restore
#line 119 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                      _exceptuados

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(146, "ExcepcionesChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Collections.Generic.List<PortalAdministrador.Services.Biometria.Models.Dedo>>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Collections.Generic.List<PortalAdministrador.Services.Biometria.Models.Dedo>>(this, 
#nullable restore
#line 119 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                                                                                        ExcepcionesChanged

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(147, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(148, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(149, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(150, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(151, "\r\n");
#nullable restore
#line 124 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Components\RegistroTramite\CapturaHuella.razor"
                           
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
