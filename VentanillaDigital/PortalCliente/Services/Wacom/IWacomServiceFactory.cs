using PortalCliente.Services.Wacom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace PortalCliente.Services.Wacom
{
    public interface IWacomServiceFactory
    {
        Task<IWacomService> GetWacomServiceInstance(string wacomChannelId);
    }
}
