#pragma checksum "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Certificado\TerminosCondiciones.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "66b28a3ac8c3301e876dd9f1979e1db891d23564"
// <auto-generated/>
#pragma warning disable 1591
namespace PortalCliente.Components.Certificado
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
    public partial class TerminosCondiciones : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h4 for=\"tyc\" class=\"mt-0 mb-2\">2. TÉRMINOS Y CONDICIONES SERVICIOS GENERADOS POR EL USUARIO</h4>\r\n");
            __builder.AddMarkupContent(1, "<div class=\"tyc\">\r\n    <div class=\"agreement\">\r\n        <h2>Generación del par de llaves </h2>\r\n        <p>\r\n            Las llaves pública y privada de titulares del certificado son generadas por el propio suscriptor. Para la emisión\r\n            del certificado debe suministrar a Olimpia IT el CSR (Certificate Signing Request).\r\n        </p>\r\n\r\n        <h2>Entrega de la llave privada al suscriptor </h2>\r\n        <p>\r\n            No procede porque la llave privada en ningún momento es conocida por Olimpia IT. La llave privada es generada por\r\n            el propio suscriptor.\r\n        </p>\r\n\r\n        <h2>Entrega de la llave pública al emisor del certificado </h2>\r\n        <p>\r\n            La entrega de la llave pública del suscriptor a Olimpia IT se realiza al registrar la solicitud de emisión de\r\n            certificado con forma de entrega “PKCS10” desde la plataforma o servicio web, donde el suscriptor vincula el CSR\r\n            que contiene la llave pública.\r\n        </p>\r\n\r\n        <h2>Distribución de la llave pública del suscriptor </h2>\r\n        <p>\r\n            La llave pública de cualquier suscriptor de certificados con forma de entrega “PKCS10” está permanentemente\r\n            disponible para descarga en la pestaña de validaciones OCSP o su estado revocado en la CRL de la página web\r\n            ecd.olimpiait.com\r\n        </p>\r\n\r\n        <h2>Distribución de la llave pública de Olimpia IT a los usuarios </h2>\r\n        <p>\r\n            La llave pública de la CA raíz de Olimpia IT, se encuentra disponible para descarga en la página WEB\r\n            ecd.olimpiait.com en la pestaña Validaciones, Certificados CA.\r\n        </p>\r\n\r\n        <h2>Periodo de utilización de la llave privada</h2>\r\n        <p>\r\n            El periodo de utilización de la llave privada es el mismo tiempo de la vigencia del certificado con forma de\r\n            entrega PKCS10 o menos si el certificado es revocado antes de caducar.\r\n        </p>\r\n        <p>\r\n            En la DPC de Olimpia IT se detalla el periodo de utilización de la llave privada de la CA raíz y las CA\r\n            subordinadas emisoras de certificados de Olimpia IT.\r\n        </p>\r\n\r\n        <h2>Tamaño de las llaves</h2>\r\n        <p>\r\n            El tamaño de las llaves de certificados con forma de entrega PKCS10 es de 2048 bits basado en el algoritmo RSA.\r\n        </p>\r\n        <p>\r\n            El tamaño de las llaves certificadas de la CA emisora de los certificados con forma de entrega PKCS10 tiene una\r\n            longitud de 4096 bits basadas en el algoritmo RSA.\r\n        </p>\r\n\r\n        <h2>Controles de protección de la llave privada </h2>\r\n        <p>\r\n            En la Declaración de Prácticas de Certificación de la entidad que recibe el CSR, para suministrar el certificado\r\n            digital se especifican los controles y estándares de los módulos criptográficos, el control, respaldo,\r\n            almacenamiento, activación, desactivación y destrucción de las llaves privadas de la Autoridad de Certificación.\r\n            <table class=\"table border\">\r\n                <thead>\r\n                    <tr>\r\n                        <th>Control de protección</th>\r\n                        <th>Llave privada generada por suscriptor CSR</th>\r\n                    </tr>\r\n                </thead>\r\n                <tbody>\r\n                    <tr>\r\n                        <td>Respaldo de la llave privada</td>\r\n                        <td>\r\n                            No realiza respaldo sobre las llaves privadas de los suscriptores cuando el certificado se genera a\r\n                            partir de CSR. La entidad que suministra el certificado digital nunca está en posesión de dichas\r\n                            llaves y solo permanecen bajo custodia del propio suscriptor.\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>Almacenamiento de la llave privada </td>\r\n                        <td>\r\n                            Las llaves privadas de los suscriptores no son almacenadas por la entidad que suministra el\r\n                            certificado digital.\r\n                            La llave privada debe ser almacenada por el propio suscriptor y la responsabilidad por su uso y la\r\n                            información cifrada con esta llave es completa del suscriptor.\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>Transferencia de la llave privada</td>\r\n                        <td>\r\n                            La llave privada generada por el usuario que solicita por CSR para generación de certificado es\r\n                            custodiada por el suscriptor y nunca es enviada a la entidad que suministra el certificado.\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>Activación de la llave privada </td>\r\n                        <td>La protección de los datos de activación es responsabilidad del suscriptor </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>Desactivación de la llave privada </td>\r\n                        <td>La desactivación de la llave privada es responsabilidad del suscriptor </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>Destrucción de llave privada </td>\r\n                        <td>\r\n                            La destrucción de la llave privada del suscriptor puede realizarla el propio suscriptor eliminando\r\n                            la llave privada correspondiente al CSR enviado a la entidad de certificación digital.\r\n                        </td>\r\n                    </tr>\r\n                </tbody>\r\n            </table>\r\n        </p>\r\n        <h2>Formato del certificado digital con forma de entrega PKCS10</h2>\r\n        <p>\r\n            El formato del certificado digital es X.509 v3 y según corresponda a la entidad de certificación digital\r\n            previamente\r\n            acreditado ante ONAC.\r\n        </p>\r\n\r\n        <h2>Términos y condiciones generales del certificado con forma de entrega PKCS10</h2>\r\n        <p>\r\n            Los demás términos y condiciones no tratados en este documento se suplen por lo dispuesto en la Declaración de\r\n            Prácticas de Certificación de la entidad de certificación que expide el certificado digital, publicada en su\r\n            página\r\n            web.\r\n        </p>\r\n        <h2>Responsabilidad</h2>\r\n        <p>\r\n            La responsabilidad por el uso ya sea legal o fuera de la ley, del par de llaves generado por el suscriptor, es de\r\n            su\r\n            entera responsabilidad, Olimpia IT y/o la entidad de certificación digital garantiza que la emisión del\r\n            certificado\r\n            digital que acompaña la información del CSR y de la llave pública generada por el suscriptor cumple con los\r\n            requerimientos técnicos exigidos para este tipo de certificado digitales según las autoridades nacionales.\r\n        </p>\r\n    </div>\r\n    </div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
