#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c985aeb906a0ec15345a7e6c48ff299d5822d5c2"
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
#line 1 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
using PortalCliente.Components.Transversales;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
using PortalCliente.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
using PortalCliente.Components.RegistroTramite.DatosAdicionales;

#line default
#line hidden
#nullable disable
    public partial class CrearTramite : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 6 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
 if (_firmaManualConfigurado)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "    ");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "class", "header-configuraciones");
            __builder.AddMarkupContent(3, "\r\n        ");
            __builder.OpenElement(4, "a");
            __builder.AddAttribute(5, "href", "/configuraciones");
            __builder.AddAttribute(6, "class", "opciones-configuracion");
            __builder.AddMarkupContent(7, "\r\n            ");
            __builder.OpenElement(8, "span");
            __builder.AddContent(9, "Firma manual activada para: ");
            __builder.OpenElement(10, "strong");
            __builder.AddContent(11, 
#nullable restore
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                       _notarioConfigurado

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(12, "\r\n            ");
            __builder.OpenElement(13, "span");
            __builder.AddMarkupContent(14, "Impresión del trámite en formato ");
            __builder.OpenElement(15, "strong");
            __builder.AddContent(16, 
#nullable restore
#line 11 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                             _usarStickerConfigurado ? "Sticker" : "Carta"

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(18, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(19, "\r\n");
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
}

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(20, "\r\n");
            __builder.OpenElement(21, "div");
            __builder.AddAttribute(22, "class", "container");
            __builder.AddMarkupContent(23, "\r\n    ");
            __builder.OpenElement(24, "div");
            __builder.AddAttribute(25, "class", "row");
            __builder.AddMarkupContent(26, "\r\n        ");
            __builder.AddMarkupContent(27, "<div class=\"col-12\">\r\n            <h1>Trámite</h1>\r\n        </div>\r\n        ");
            __builder.OpenElement(28, "div");
            __builder.AddAttribute(29, "class", "col-sm-12 col-md-6 order col-tablet");
            __builder.AddMarkupContent(30, "\r\n            ");
            __builder.AddMarkupContent(31, "<h3>Selección de trámite </h3>\r\n            ");
            __builder.OpenComponent<PortalCliente.Components.Transversales.RBSelector>(32);
            __builder.AddAttribute(33, "Seleccion", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(
#nullable restore
#line 23 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                   Tramite.TipoTramite

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(34, "SeleccionChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Object>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Object>(this, 
#nullable restore
#line 24 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                            async (v)=> await ChildChanged("TipoTramite",v)

#line default
#line hidden
#nullable disable
            )));
            __builder.AddAttribute(35, "Class", "tipo-tramites-container");
            __builder.AddAttribute(36, "Name", "tramites");
            __builder.AddAttribute(37, "Categorias", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object[]>(
#nullable restore
#line 25 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                    categorias

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(38, "Opciones", new System.Func<System.Object, System.Collections.Generic.IEnumerable<System.Object>>(
#nullable restore
#line 25 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                          c=>((Categoria)c).TiposTramites

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(39, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(40, "\r\n        ");
            __builder.OpenElement(41, "div");
            __builder.AddAttribute(42, "class", "col-sm-12 col-md-6 col-tablet");
            __builder.AddMarkupContent(43, "\r\n");
#nullable restore
#line 29 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
             if (Tramite.TipoTramite != null)
            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(44, "                ");
            __builder.AddMarkupContent(45, "<h3>Detalles trámite</h3>\r\n");
#nullable restore
#line 33 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                 if (_esAndroid)
                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(46, "                    ");
            __builder.OpenElement(47, "fieldset");
            __builder.AddMarkupContent(48, "\r\n                        ");
            __builder.AddMarkupContent(49, "<legend>Lugar de comparecencia</legend>\r\n                        ");
            __builder.OpenElement(50, "label");
            __builder.AddAttribute(51, "class", "radio-button-container");
            __builder.AddAttribute(52, "for", "EnNotaria");
            __builder.AddMarkupContent(53, "\r\n                            Trámite en Notaría\r\n                            ");
            __builder.OpenElement(54, "input");
            __builder.AddAttribute(55, "class", "radio-input");
            __builder.AddAttribute(56, "type", "radio");
            __builder.AddAttribute(57, "id", "EnNotaria");
            __builder.AddAttribute(58, "name", "lugarTramite");
            __builder.AddAttribute(59, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 39 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                                                                    () => UpdateLugar(1)

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(60, "\r\n                            <span class=\"radio-active\"></span>\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(61, "\r\n\r\n                        ");
            __builder.OpenElement(62, "label");
            __builder.AddAttribute(63, "class", "radio-button-container");
            __builder.AddAttribute(64, "for", "FueraDespacho");
            __builder.AddMarkupContent(65, "\r\n                            Trámite fuera del despacho\r\n                            ");
            __builder.OpenElement(66, "input");
            __builder.AddAttribute(67, "class", "radio-input");
            __builder.AddAttribute(68, "type", "radio");
            __builder.AddAttribute(69, "id", "FueraDespacho");
            __builder.AddAttribute(70, "name", "lugarTramite");
            __builder.AddAttribute(71, "checked", true);
            __builder.AddAttribute(72, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 45 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                                                                                () => UpdateLugar(2)

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(73, "\r\n                            <span class=\"radio-active\"></span>\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(74, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n");
#nullable restore
#line 49 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"


                    if (@lugarComparecencia == "FueraDespacho")
                    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(76, "                        ");
            __builder.OpenElement(77, "div");
            __builder.AddAttribute(78, "class", "input-container");
            __builder.AddMarkupContent(79, "\r\n                            ");
            __builder.AddMarkupContent(80, "<label>Dirección</label>\r\n                            ");
            __builder.OpenElement(81, "input");
            __builder.AddAttribute(82, "type", "text");
            __builder.AddAttribute(83, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 55 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                             direccionComparecencia

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(84, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => direccionComparecencia = __value, direccionComparecencia));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(85, "\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(86, "\r\n");
#nullable restore
#line 57 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                    }
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(87, "                ");
            __builder.AddMarkupContent(88, "<label>Cantidad de comparecientes</label>\r\n                ");
            __builder.OpenElement(89, "div");
            __builder.AddAttribute(90, "class", "cantidad-comp-container");
            __builder.AddMarkupContent(91, "\r\n                    ");
            __builder.OpenElement(92, "button");
            __builder.AddAttribute(93, "type", "button");
            __builder.AddAttribute(94, "class", "btn-resta");
            __builder.AddAttribute(95, "data-quantity", "resta");
            __builder.AddAttribute(96, "id", "restarCompareciente");
            __builder.AddAttribute(97, "disabled", 
#nullable restore
#line 62 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                       _bloquearNumeroComparecientes

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(98, "data-field", "cantidad-comparecientes");
            __builder.AddAttribute(99, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 63 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                           RestarCompareciente

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(100, "\r\n                        <i class=\"fa fa-minus\" aria-hidden=\"true\"></i>\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(101, "\r\n                    ");
            __builder.OpenElement(102, "input");
            __builder.AddAttribute(103, "type", "text");
            __builder.AddAttribute(104, "pattern", "\\d*");
            __builder.AddAttribute(105, "id", "cantidadComparecientes");
            __builder.AddAttribute(106, "class", "comparecientes-input");
            __builder.AddAttribute(107, "maxlength", "2");
            __builder.AddAttribute(108, "value", 
#nullable restore
#line 67 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                   Tramite.CantidadComparecientes

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(109, "disabled", 
#nullable restore
#line 68 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                      _bloquearNumeroComparecientes

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(110, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 69 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                       async (v)=> await ChildChanged("CantidadComparecientes",v.Value)

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.AddMarkupContent(111, "\r\n                    ");
            __builder.OpenElement(112, "button");
            __builder.AddAttribute(113, "type", "button");
            __builder.AddAttribute(114, "class", "btn-suma");
            __builder.AddAttribute(115, "data-quantity", "suma");
            __builder.AddAttribute(116, "id", "sumarCompareciente");
            __builder.AddAttribute(117, "disabled", 
#nullable restore
#line 71 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                       _bloquearNumeroComparecientes

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(118, "data-field", "cantidad-comparecientes");
            __builder.AddAttribute(119, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 72 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                           SumarCompareciente

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(120, "\r\n                        <i class=\"fa fa-plus\" aria-hidden=\"true\"></i>\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(121, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(122, "\r\n");
#nullable restore
#line 76 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                 switch ((CodigoTipoTramite)Tramite.TipoTramite.CodigoTramite)
                {
                    case CodigoTipoTramite.DocumentoPrivado:

#line default
#line hidden
#nullable disable
            __builder.AddContent(123, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.DocumentoPrivado>(124);
            __builder.AddAttribute(125, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 79 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                     SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(126, "\r\n");
#nullable restore
#line 80 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.EscrituraPublica:

#line default
#line hidden
#nullable disable
            __builder.AddContent(127, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.EscrituraPublica>(128);
            __builder.AddAttribute(129, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 82 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                     SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(130, "\r\n");
#nullable restore
#line 83 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.DeclaracionExtraProceso:

#line default
#line hidden
#nullable disable
            __builder.AddContent(131, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.DeclaracionExtraProceso>(132);
            __builder.AddAttribute(133, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 85 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                            SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(134, "\r\n");
#nullable restore
#line 86 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.PresentacionPersonal:

#line default
#line hidden
#nullable disable
            __builder.AddContent(135, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.PresentacionPersonal>(136);
            __builder.AddAttribute(137, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 88 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                         SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(138, "\r\n");
#nullable restore
#line 89 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.EnrolamientoNotariaDigital:

#line default
#line hidden
#nullable disable
            __builder.AddContent(139, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.AutenticacionHuella>(140);
            __builder.AddAttribute(141, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 91 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                        SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(142, "\r\n");
#nullable restore
#line 92 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.InscripcionRegistroCivil:

#line default
#line hidden
#nullable disable
            __builder.AddContent(143, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.InscripcionRegistroCivil>(144);
            __builder.AddAttribute(145, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 94 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                             SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(146, "\r\n");
#nullable restore
#line 95 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.DocumentoPrivadoFirmaARuego:

#line default
#line hidden
#nullable disable
            __builder.AddContent(147, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.DocumentoPrivadoFirmaARuego>(148);
            __builder.AddAttribute(149, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 97 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(150, "\r\n");
#nullable restore
#line 98 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.DocumentoPrivadoInvidente:

#line default
#line hidden
#nullable disable
            __builder.AddContent(151, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.DocumentoPrivadoInvidente>(152);
            __builder.AddAttribute(153, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 100 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                              SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.AddAttribute(154, "NoSabeFirmarChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Boolean>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Boolean>(this, 
#nullable restore
#line 100 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                                                         NoSabeFirmarChanged

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(155, "\r\n");
#nullable restore
#line 101 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                    case CodigoTipoTramite.AutenticacionFirma:

#line default
#line hidden
#nullable disable
            __builder.AddContent(156, "                        ");
            __builder.OpenComponent<PortalCliente.Components.RegistroTramite.DatosAdicionales.AutenticacionFirma>(157);
            __builder.AddAttribute(158, "GetFields", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, 
#nullable restore
#line 103 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                       SetCamposAdicionales

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(159, "\r\n");
#nullable restore
#line 104 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        break;
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(160, "                ");
            __builder.OpenElement(161, "div");
            __builder.AddAttribute(162, "class", "btn-crear-tramite-container");
            __builder.AddMarkupContent(163, "\r\n                    ");
            __builder.OpenElement(164, "button");
            __builder.AddAttribute(165, "class", "btn-primario btn-crear-tramite");
            __builder.AddAttribute(166, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 107 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                             Crear

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(167, "disabled", 
#nullable restore
#line 107 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                                                                                                Tramite.TipoTramite == null || showSpinner

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(168, "\r\n");
#nullable restore
#line 108 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                         if (showSpinner)
                        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(169, "                            <i class=\"fa fa-sync fa-spin\"></i>\r\n");
#nullable restore
#line 111 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(170, "                            <i class=\"fas fa-plus-circle mr-2\"></i>\r\n");
#nullable restore
#line 115 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
                        }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(171, "                        Crear trámite\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(172, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(173, "\r\n");
#nullable restore
#line 119 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\RegistroTramite\CrearTramite.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(174, "        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(175, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(176, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(177, "\r\n\r\n\r\n");
            __builder.AddMarkupContent(178, @"<div class=""modal fade"" id=""sesionClosedModalCenter"" tabindex=""-1"" role=""dialog"" aria-labelledby=""sesionClosedModalCenter"" aria-hidden=""true"">
    <div class=""modal-dialog modal-sm modal-dialog-centered"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h3>Su sesión ha caducado.</h3>
            </div>
            <div class=""modal-body"">
                <p>Se le redigirá al inicio de sesión...</p>
            </div>
        </div>
    </div>
</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime js { get; set; }
    }
}
#pragma warning restore 1591
