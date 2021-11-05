using Dominio.ContextoPrincipal.Entidad.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.ContextoPrincipal.ContratoRepositorio.StoredProcedures
{
    public interface IProcedimientoAlmacenadoRepositorio
    {
        Task<Tuple<List<TramitesPendientes>, int>> TramitesPendientes_Obtener(string bodyJson);
    }
}
