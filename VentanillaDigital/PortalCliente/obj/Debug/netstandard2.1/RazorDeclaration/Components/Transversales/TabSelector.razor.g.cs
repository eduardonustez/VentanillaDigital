// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
    public partial class TabSelector : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 14 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
       

    [Parameter]
    public RenderFragment Tabs { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get => Tabs; set => Tabs = value; }


    [Parameter]
    public RenderFragment<RenderFragment> Navigator { get; set; }

    [Parameter]
    public RenderFragment<Tab> NavigatorLink { get; set; }



    protected override async Task OnParametersSetAsync()
    {
        if (NavigatorLink == null)
        {
            NavigatorLink = tab =>

#line default
#line hidden
#nullable disable
        (__builder2) => {
            __builder2.AddContent(0, "        ");
            __builder2.OpenElement(1, "li");
            __builder2.AddAttribute(2, "class", "nav-item");
            __builder2.AddMarkupContent(3, "\r\n            ");
            __builder2.OpenElement(4, "button");
            __builder2.AddAttribute(5, "@key", "tab");
            __builder2.AddAttribute(6, "@onclick", "async () => await tab.Parent.Select(tab.Name)");
            __builder2.AddAttribute(7, "class", "nav-link" + " " + (
#nullable restore
#line 38 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
                                      tab.Active ? "active show" : ""

#line default
#line hidden
#nullable disable
            ) + " " + (
#nullable restore
#line 38 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
                                                                         tab.Disabled ? "disabled" : ""

#line default
#line hidden
#nullable disable
            ));
            __builder2.AddAttribute(8, "tabindex", 
#nullable restore
#line 39 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
                                tab.Disabled ? -1 : 0

#line default
#line hidden
#nullable disable
            );
            __builder2.AddAttribute(9, "aria-disabled", 
#nullable restore
#line 39 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
                                                                        tab.Disabled

#line default
#line hidden
#nullable disable
            );
            __builder2.AddMarkupContent(10, "\r\n                ");
            __builder2.AddContent(11, 
#nullable restore
#line 40 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
                 tab.Selector

#line default
#line hidden
#nullable disable
            );
            __builder2.AddMarkupContent(12, "\r\n            ");
            __builder2.CloseElement();
            __builder2.AddMarkupContent(13, "\r\n        ");
            __builder2.CloseElement();
            __builder2.AddMarkupContent(14, "\r\n");
            __builder2.AddMarkupContent(15, "\r\n");
        }
#nullable restore
#line 44 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
            ;
        }

        if (Navigator == null)
        {
            Navigator = tabs =>

#line default
#line hidden
#nullable disable
        (__builder2) => {
            __builder2.AddContent(16, "        ");
            __builder2.OpenElement(17, "ul");
            __builder2.AddAttribute(18, "class", "nav nav-tabs");
            __builder2.AddMarkupContent(19, "\r\n            ");
            __builder2.AddContent(20, 
#nullable restore
#line 51 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
             tabs

#line default
#line hidden
#nullable disable
            );
            __builder2.AddMarkupContent(21, "\r\n        ");
            __builder2.CloseElement();
            __builder2.AddMarkupContent(22, "\r\n");
            __builder2.AddMarkupContent(23, "\r\n");
        }
#nullable restore
#line 54 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalCliente\Components\Transversales\TabSelector.razor"
            ;
        }

        await base.OnParametersSetAsync();
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
