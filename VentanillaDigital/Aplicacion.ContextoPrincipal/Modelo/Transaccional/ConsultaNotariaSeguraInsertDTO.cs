using System;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ConsultaNotariaSeguraInsertDTO
    {
        public string Nut
        {
            get;
            set;
        }

        public string NutHash
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

        public bool SeEncontroArchivo
        {
            get;
            set;
        }
    }
}