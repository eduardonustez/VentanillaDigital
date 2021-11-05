using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.DescriptorCliente
{
    public interface IDescriptorCliente
    {
        public Task<bool> EsMovil { get; }
    }
}
