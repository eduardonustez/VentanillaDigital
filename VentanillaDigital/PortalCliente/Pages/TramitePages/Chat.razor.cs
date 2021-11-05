using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using PortalCliente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.TramitePages
{
    public class MensajeTramite
    {
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public bool Notario { get; set; }
    }

    public partial class Chat
    {
        [Parameter] public long TramitePortalVirtualId { get; set; }
        [Inject]
        public ITramiteVirtualService tramitesVirtualService { get; set; }
        public List<TramiteVirtualMensajeModel> Mensajes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ConsultarMensajesTramite(TramitePortalVirtualId);
        }

        async Task ConsultarMensajesTramite(long tramitePortalVirtualId)
        {
            var mensajes = await tramitesVirtualService.ConsultarMensajesTramiteVirtual(tramitePortalVirtualId);
            Mensajes = mensajes?.ToList();
        }
    }
}
