// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace PortalAdministrador.Shared
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
#line 10 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Shared\MainLayout.razor"
using PortalAdministrador.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Shared\MainLayout.razor"
using PortalAdministrador.Data.Account;

#line default
#line hidden
#nullable disable
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 160 "D:\VentanillaDigitalGit\Web.Application.NotariaFijaYMovil\VentanillaDigital\PortalAdministrador\Shared\MainLayout.razor"
      
    string menuIconoDefecto = "menu";
    string nombreNotaria;
    public string ModalDisplay = "none;";
    public string ModalClass = "";
    public bool ShowBackdrop = false;
    public string MensajeCerrarSesion = "¿Desea cerrar sesión? Recuerde guardar cambios.";
    double tiempoInactividad = 0;
    DotNetObjectReference<MainLayout> ObjectReference;
    async void ExpandirNav()
    {
        menuIconoDefecto = menuIconoDefecto == "menu" ? "menu_open" : "menu";
        await js.InvokeVoidAsync("expandirNav");
    }
    //[CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
    [JSInvokable]
    public async Task Logout()
    {
        await
            ((PortalAdministrador.Services.CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsLoggedOut();
        NavigationManager.NavigateTo("/login");
    }
    [JSInvokable]
    public async Task notificationLogout()
    {
        SetMensajeCerrarSesion();
        Open();
    }

    protected override async Task OnInitializedAsync()
    {
        ObjectReference = DotNetObjectReference.Create(this);
        var userActual = (CustomAuthenticationStateProvider)_authenticationStateProvider;
        AuthenticatedUser userName = await userActual.GetAuthenticatedUser();
        if (userName != null)
        {
            await LogOutDueToInactivity(userName.Rol);
        }

        await RegistrarMaquinaAsync();
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
            //NavigationManager.NavigateTo("/install");
            Console.WriteLine("👮🏼‍ Agente no encontrado");
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

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PortalAdministrador.Services.Notaria.INotariaService NotariaService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IParametrizacionServicio Parametrizacion { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IConfiguration Configuration { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime js { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    }
}
#pragma warning restore 1591
