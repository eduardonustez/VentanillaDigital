using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Adaptador
{
    public interface IAdaptadorTipoFactory
    {
        IAdaptadorTipo Create();
    }
}
