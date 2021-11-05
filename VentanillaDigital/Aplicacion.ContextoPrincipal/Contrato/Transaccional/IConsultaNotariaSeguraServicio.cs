using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Transaccional
{
    public interface IConsultaNotariaSeguraServicio:IDisposable
    {
        Task<string> InsertarConsultaNotariaSegura(ConsultaNotariaSeguraInsertDTO consultaNotariaSeguraInsert);
    }
}