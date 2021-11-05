using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Contratos
{
    public interface ISerilogFactory
    {
        ISerilog Create();
    }
}
