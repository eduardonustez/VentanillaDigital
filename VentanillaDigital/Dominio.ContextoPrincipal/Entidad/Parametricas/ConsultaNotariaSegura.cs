using Dominio.Nucleo.Entidad;
using System;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class ConsultaNotariaSegura:EntidadBase
    {
        public long ConsultaNotariaSeguraId
        {
            get;
            set;
        }

        public string TramiteId
        {
            get;
            set;
        }

        public string TramiteIdHash
        {
            get;
            set;
        }

        public long NotariaId
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public DateTime FechaConsulta
        {
            get;
            set;
        }

        public bool EncontroArchivo
        {
            get;
            set;
        }
    }
}