#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e9316ef4439cd639de00e4370c3059ee81d186fe"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalCliente.Shared
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
#line 13 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
using Components.RegistroTramite;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
using PortalCliente.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
using PortalCliente.Data.Account;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
using ApiGateway.Models.Transaccional;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
using PortalCliente.Pages.TramitePages;

#line default
#line hidden
#nullable disable
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __Blazor.PortalCliente.Shared.MainLayout.TypeInference.CreateCascadingValue_0(__builder, 0, 1, 
#nullable restore
#line 19 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                       this

#line default
#line hidden
#nullable disable
            , 2, (__builder2) => {
                __builder2.AddMarkupContent(3, "\r\n    ");
                __builder2.OpenElement(4, "header");
                __builder2.AddMarkupContent(5, "\r\n");
#nullable restore
#line 21 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
         if (menuFijas)
        {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(6, "            ");
                __builder2.OpenElement(7, "div");
                __builder2.AddAttribute(8, "class", "logo-main-menu");
                __builder2.AddAttribute(9, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 23 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                  ExpandirNav

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddMarkupContent(10, "\r\n                <img src=\"images/logo-ucnc.svg\" class=\"header-logo-menu\" alt=\"UCNC logo\">\r\n            ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(11, "\r\n");
#nullable restore
#line 26 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
        }

#line default
#line hidden
#nullable disable
                __builder2.AddContent(12, "        ");
                __builder2.OpenElement(13, "p");
                __builder2.AddAttribute(14, "class", "titulo-header");
                __builder2.AddContent(15, 
#nullable restore
#line 27 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                  nombreNotaria

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(16, "\r\n");
#nullable restore
#line 28 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
         if (esNotariaVirtual)
        {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(17, "            ");
                __builder2.AddMarkupContent(18, "<a id=\"tramites-virtuales-link\" href=\"/tramitesportalvirtual\">Ir a trámites virtuales</a>\r\n");
#nullable restore
#line 31 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
        }

#line default
#line hidden
#nullable disable
                __builder2.AddContent(19, "        ");
                __builder2.OpenElement(20, "div");
                __builder2.AddAttribute(21, "class", "mini-profile-info");
                __builder2.AddMarkupContent(22, "\r\n            ");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(23);
                __builder2.AddAttribute(24, "Authorized", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder3) => {
                    __builder3.AddMarkupContent(25, "\r\n                    ");
                    __builder3.OpenElement(26, "span");
                    __builder3.AddAttribute(27, "class", "mini-profile-info-header");
                    __builder3.AddContent(28, 
#nullable restore
#line 35 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                            context.User.Identity.Name

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(29, "\r\n                    ");
                    __builder3.OpenElement(30, "small");
                    __builder3.AddAttribute(31, "class", "header-close-session");
                    __builder3.AddAttribute(32, "title", "Cerrar sesión");
                    __builder3.AddAttribute(33, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 36 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                                        Open

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddMarkupContent(34, "Cerrar sesión");
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(35, "\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(36, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(37, "\r\n    ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(38, "\r\n    ");
                __builder2.OpenElement(39, "div");
                __builder2.AddAttribute(40, "class", "ParentMainLayout");
                __builder2.AddMarkupContent(41, "\r\n        ");
                __builder2.OpenElement(42, "div");
                __builder2.AddAttribute(43, "class", "left-sidebar");
                __builder2.AddMarkupContent(44, "\r\n            ");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(45);
                __builder2.AddAttribute(46, "NotAuthorized", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder3) => {
                    __builder3.AddMarkupContent(47, "\r\n                    ");
                    __builder3.OpenElement(48, "div");
                    __builder3.AddAttribute(49, "class", "mensaje-error");
                    __builder3.AddMarkupContent(50, "\r\n                        ");
                    __builder3.AddMarkupContent(51, "<p>Usted no ha iniciado sesión (o su sesión caducó).</p>\r\n                        ");
                    __builder3.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(52);
                    __builder3.AddAttribute(53, "class", "nav-link");
                    __builder3.AddAttribute(54, "href", "Login");
                    __builder3.AddAttribute(55, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(56, "\r\n                            Iniciar sesión\r\n                        ");
                    }
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(57, "\r\n                    ");
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(58, "\r\n                ");
                }
                ));
                __builder2.AddAttribute(59, "Authorized", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder3) => {
                    __builder3.AddMarkupContent(60, "\r\n\r\n                    ");
                    __builder3.OpenElement(61, "nav");
                    __builder3.AddAttribute(62, "class", "main-menu");
                    __builder3.AddMarkupContent(63, "\r\n                        ");
                    __builder3.OpenElement(64, "ul");
                    __builder3.AddAttribute(65, "class", "main-menu-botones");
                    __builder3.AddMarkupContent(66, "\r\n                            ");
                    __builder3.OpenElement(67, "label");
                    __builder3.AddMarkupContent(68, "\r\n                                ");
                    __builder3.OpenElement(69, "span");
                    __builder3.AddAttribute(70, "title", "Expandir menú");
                    __builder3.AddAttribute(71, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 57 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                      ExpandirNav

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(72, "class", "icono-en-menu material-icons");
                    __builder3.AddMarkupContent(73, "\r\n                                    ");
                    __builder3.AddContent(74, 
#nullable restore
#line 58 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                     menuIconoDefecto

#line default
#line hidden
#nullable disable
                    );
                    __builder3.AddMarkupContent(75, "\r\n                                ");
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(76, "\r\n                            ");
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(77, "\r\n");
#nullable restore
#line 61 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                             if (menuFijas)
                            {
                                

#line default
#line hidden
#nullable disable
#nullable restore
#line 63 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                 if (context.User.IsInRole("Administrador") || context.User.IsInRole("Notario Encargado"))
                                {

#line default
#line hidden
#nullable disable
                    __builder3.AddContent(78, "                                    ");
                    __builder3.OpenComponent<PortalCliente.Shared.NavNotario>(79);
                    __builder3.AddAttribute(80, "onListenClickMenu", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 65 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                   OpenCancelTramiteModal

#line default
#line hidden
#nullable disable
                    )));
                    __builder3.AddAttribute(81, "usePreventDefault", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 66 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                   TramiteIdEnProceso > 0

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(82, "\r\n");
#nullable restore
#line 67 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                }
                                else
                                {

#line default
#line hidden
#nullable disable
                    __builder3.AddContent(83, "                                    ");
                    __builder3.OpenComponent<PortalCliente.Shared.NavOperario>(84);
                    __builder3.AddAttribute(85, "onListenClickMenu", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 70 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                    OpenCancelTramiteModal

#line default
#line hidden
#nullable disable
                    )));
                    __builder3.AddAttribute(86, "usePreventDefault", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 71 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                    TramiteIdEnProceso > 0

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(87, "\r\n");
#nullable restore
#line 73 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 73 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                 
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
                    __builder3.AddContent(88, "                                ");
                    __builder3.OpenElement(89, "NavNotariaDigital");
                    __builder3.AddAttribute(90, "NotariaConvenioVirtual", "esNotariaVirtual");
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(91, "\r\n");
#nullable restore
#line 78 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                            }

#line default
#line hidden
#nullable disable
                    __builder3.AddMarkupContent(92, "\r\n                        ");
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(93, "\r\n\r\n                    ");
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(94, "\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(95, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(96, "\r\n        ");
                __builder2.OpenElement(97, "main");
                __builder2.AddAttribute(98, "class", "wrapper");
                __builder2.AddMarkupContent(99, "\r\n            ");
                __builder2.OpenElement(100, "div");
                __builder2.AddAttribute(101, "id", "content");
                __builder2.AddAttribute(102, "class", "main container-fluid content");
                __builder2.AddMarkupContent(103, "\r\n                ");
                __builder2.OpenElement(104, "div");
                __builder2.AddAttribute(105, "class", "contenido_principal");
                __builder2.AddMarkupContent(106, "\r\n                    ");
                __builder2.OpenComponent<Radzen.Blazor.RadzenNotification>(107);
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(108, "\r\n                    ");
                __builder2.OpenComponent<Radzen.Blazor.RadzenDialog>(109);
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(110, "\r\n\r\n                    ");
                __builder2.AddContent(111, 
#nullable restore
#line 92 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                     Body

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(112, "\r\n                ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(113, "\r\n            ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(114, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(115, "\r\n    ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(116, "\r\n");
            }
            );
            __builder.AddMarkupContent(117, "\r\n\r\n");
            __builder.OpenElement(118, "div");
            __builder.AddAttribute(119, "class", "modal" + " " + (
#nullable restore
#line 99 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                   ModalClass

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(120, "id", "avisoTramiteModal");
            __builder.AddAttribute(121, "tabindex", "-1");
            __builder.AddAttribute(122, "role", "dialog");
            __builder.AddAttribute(123, "aria-labelledby", "avisoTramiteModalLabel");
            __builder.AddAttribute(124, "aria-hidden", "true");
            __builder.AddAttribute(125, "style", "display:" + (
#nullable restore
#line 100 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                                 ModalDisplay

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(126, "\r\n    ");
            __builder.OpenElement(127, "div");
            __builder.AddAttribute(128, "class", "modal-dialog modal-dialog-centered");
            __builder.AddAttribute(129, "role", "document");
            __builder.AddMarkupContent(130, "\r\n        ");
            __builder.OpenElement(131, "div");
            __builder.AddAttribute(132, "class", "modal-content");
            __builder.AddMarkupContent(133, "\r\n            ");
            __builder.AddMarkupContent(134, "<div class=\"modal-header\">\r\n                <h3>Espere un momento...</h3>\r\n            </div>\r\n            ");
            __builder.OpenElement(135, "div");
            __builder.AddAttribute(136, "class", "modal-body");
            __builder.AddMarkupContent(137, "\r\n                ");
            __builder.OpenElement(138, "p");
            __builder.AddContent(139, 
#nullable restore
#line 107 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                    MensajeCerrarSesion

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(140, "\r\n                ");
            __builder.OpenElement(141, "div");
            __builder.AddAttribute(142, "class", "d-flex justify-content-around mt-3");
            __builder.AddMarkupContent(143, "\r\n\r\n                    ");
            __builder.OpenElement(144, "button");
            __builder.AddAttribute(145, "type", "button");
            __builder.AddAttribute(146, "class", "btn-contorno-primario");
            __builder.AddAttribute(147, "data-dismiss", "modal");
            __builder.AddAttribute(148, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 110 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                                                       Close

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(149, "\r\n                        Volver a\r\n                        la sesión\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(150, "\r\n                    ");
            __builder.OpenElement(151, "button");
            __builder.AddAttribute(152, "type", "button");
            __builder.AddAttribute(153, "class", "btn-primario");
            __builder.AddAttribute(154, "data-dismiss", "modal");
            __builder.AddAttribute(155, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 114 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                                              Logout

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(156, "\r\n                        Cerrar sesión\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(157, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(158, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(159, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(160, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(161, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(162, "\r\n");
#nullable restore
#line 122 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
 if (ShowBackdrop)
{

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(163, "    <div class=\"modal-backdrop fade show\"></div>\r\n");
#nullable restore
#line 125 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
}

#line default
#line hidden
#nullable disable
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.CancelarTramiteModal>(164);
            __builder.AddAttribute(165, "MotivoCancelacionChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 127 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                                         CancelTramite

#line default
#line hidden
#nullable disable
            )));
            __builder.AddComponentReferenceCapture(166, (__value) => {
#nullable restore
#line 126 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                                                                     cancelarTramiteModal = (PortalCliente.Components.RegistroTramite.CancelarTramiteModal)__value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseComponent();
            __builder.AddMarkupContent(167, "\r\n");
            __builder.OpenComponent<PortalCliente.Components.Transversales.ModalForm>(168);
            __builder.AddAttribute(169, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(170, "\r\n    <img src=\"./images/MyPost.jpg\">\r\n");
            }
            ));
            __builder.AddComponentReferenceCapture(171, (__value) => {
#nullable restore
#line 128 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
                 modalFormAnuncio = (PortalCliente.Components.Transversales.ModalForm)__value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 131 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Shared\MainLayout.razor"
      

    public long TramiteIdEnProceso { get; set; }
    private CancelarTramiteModal cancelarTramiteModal { get; set; }
    public TramitePage tramitePage { get; set; }
    string menuIconoDefecto = "menu";
    bool menuFijas = true;
    string nombreNotaria;
    public string ModalDisplay = "none;";
    public string ModalClass = "";
    public bool ShowBackdrop = false;
    public string MensajeCerrarSesion = "¿Desea cerrar sesión? Recuerde guardar cambios.";
    double tiempoInactividad = 0;
    string lastRequestedUrl = "";
    string AnuncioSitio = "";
    DotNetObjectReference<MainLayout> ObjectReference;
    [Inject]
    public ILocalStorageService localStorageService { get; set; }

    ModalForm modalFormAnuncio { get; set; }
    public bool esNotariaVirtual { get; set; }
    async void ExpandirNav()
    {
        menuIconoDefecto = menuIconoDefecto == "menu" ? "menu_open" : "menu";
        await js.InvokeVoidAsync("expandirNav");
    }
    void ChangeMenu(bool menu)
    {
        menuFijas = menu;
    }

    //[CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
    [JSInvokable]
    public async Task Logout()
    {
        await ((PortalCliente.Services.CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsLoggedOut();
        NavigationManager.NavigateTo("/login");
    }
    [JSInvokable]
    public async Task notificationLogout()
    {
        SetMensajeCerrarSesion();
        Open();
    }
    public async Task ModificarVisuales()
    {
        await js.InvokeVoidAsync("cambiarTheme");
        if (NavigationManager.Uri.Contains("virtual") || NavigationManager.Uri.Contains("ciudadano"))
        {
            menuFijas = false;
        }
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        ObjectReference = DotNetObjectReference.Create(this);
        var userActual = (CustomAuthenticationStateProvider)_authenticationStateProvider;
        AuthenticatedUser userName = await userActual.GetAuthenticatedUser();
        nombreNotaria = userName != null ? (await
            NotariaService.ObtenerNotariaPorId(long.Parse(userName.Notaria))).NotariaNombre : "";
        if (userName != null)
        {
            nombreNotaria = (await
                NotariaService.ObtenerNotariaPorId(long.Parse(userName.Notaria))).NotariaNombre;
            await LogOutDueToInactivity(userName.Rol);
        }
        await ValidarConvenioNotariaVirtual();
        await _parametrizacionServicio.ValidarEstadoCaptorHuella();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            AnuncioSitio = Configuration.GetValue<string>("AnuncioSitio");
            if (AnuncioSitio == "1")
                modalFormAnuncio.Open();
        }
    }

    async Task LogOutDueToInactivity(string rolName)
    {
        if (rolName == "Administrador" || rolName == "Notario Encargado")
        {
            tiempoInactividad = Configuration.GetValue<double>("SessionTimeMinutesAdmin");
            await js.InvokeVoidAsync("timerInactivo", ObjectReference,
                tiempoInactividad);
        }
        else
        {
            tiempoInactividad = Configuration.GetValue<double>("SessionTimeMinutes");
            await js.InvokeVoidAsync("timerInactivo", ObjectReference,
                tiempoInactividad);
        }
    }
    void SetMensajeCerrarSesion()
    {
        if (tiempoInactividad >= 3)
            MensajeCerrarSesion = "Su sesión vencerá en 2 minutos. Recuerde finalizar el trámite en proceso o cerrar sesión.";
        else if (tiempoInactividad >= 1 && tiempoInactividad < 3)
            MensajeCerrarSesion = "Su sesión vencerá en 30 segundos. Recuerde finalizar el trámite en proceso o cerrar sesión.";
        else
            MensajeCerrarSesion = "¿Desea cerrar sesión? Recuerde guardar cambios.";
    }

    async void MostrarMenu()
    {
        await js.InvokeVoidAsync("mostrarMenu");
    }

    async Task RegistrarMaquinaAsync()
    {
        try
        {
            await Parametrizacion.RegistrarMaquina();
        }
        catch (ApplicationException)
        {
            NavigationManager.NavigateTo("/install");
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (ObjectReference != null)
        {
            //Now dispose our object reference so our component can be garbage collected
            ObjectReference.Dispose();
        }
    }
    public void Open()
    {
        ModalDisplay = "block;";
        ModalClass = "Show";
        ShowBackdrop = true;
        StateHasChanged();
    }

    public void Close()
    {
        ModalDisplay = "none";
        ModalClass = "";
        ShowBackdrop = false;
        MensajeCerrarSesion = "¿Desea cerrar sesión? Recuerde guardar cambios.";
        StateHasChanged();
    }

    protected async void OpenCancelTramiteModal(string urlRedirect)
    {
        lastRequestedUrl = urlRedirect;
        if (TramiteIdEnProceso > 0)
            cancelarTramiteModal.Open();
    }
    protected async void CancelTramite(string MotivoCancelacion)
    {
        if (!string.IsNullOrWhiteSpace(MotivoCancelacion))
        {
            TramiteIdEnProceso = 0;
            await tramitePage.CancelarTramite(MotivoCancelacion);
            if (!string.IsNullOrWhiteSpace(lastRequestedUrl))
                NavigationManager.NavigateTo(lastRequestedUrl);
        }
    }

    protected async Task ValidarConvenioNotariaVirtual()
    {
        string[] res = { };
        var value = await localStorageService.GetItem<string>("ConvenioNotaria");
        if (!string.IsNullOrEmpty(value))
            res = value.Split('|');
        if (res.Length == 0)
        {
            await AsignarConvenioNotariaVirtual();
        }
        else
        {
            bool.TryParse(res[0], out bool ConvenioNotaria);
            DateTime.TryParse(res[1], out DateTime fechaConsulta);
            if (fechaConsulta < DateTime.Now)
                await AsignarConvenioNotariaVirtual();
            else
                esNotariaVirtual = ConvenioNotaria;
        }
    }

    protected async Task AsignarConvenioNotariaVirtual()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var authuser = state.User;
        if (!long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out long notariaId))
            notariaId = -1;
        if (notariaId != -1)
        {
            esNotariaVirtual = await _tramitesVirtualService.ValidarConvenioNotariaVirtual(notariaId);
            await localStorageService.RemoveItem("ConvenioNotaria");
            await localStorageService.SetItem<string>("ConvenioNotaria", $"{esNotariaVirtual.ToString()}|{DateTime.Now.AddDays(1)}");
        }
        else
        {
            //await localStorageService.RemoveItem("ConvenioNotaria");
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IParametrizacionServicio _parametrizacionServicio { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITramiteVirtualService _tramitesVirtualService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PortalCliente.Services.SignalR.ISignalRv2Service SignalRv2Service { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PortalCliente.Services.Notaria.INotariaService NotariaService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IParametrizacionServicio Parametrizacion { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IConfiguration Configuration { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime js { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    }
}
namespace __Blazor.PortalCliente.Shared.MainLayout
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateCascadingValue_0<TValue>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TValue __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Microsoft.AspNetCore.Components.CascadingValue<TValue>>(seq);
        __builder.AddAttribute(__seq0, "Value", __arg0);
        __builder.AddAttribute(__seq1, "ChildContent", __arg1);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591