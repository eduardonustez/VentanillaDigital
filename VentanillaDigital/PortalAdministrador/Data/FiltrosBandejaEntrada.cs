using System;

namespace PortalAdministrador.Data
{
    public class FiltrosBandejaEntrada
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public long NuipOperador { get; set; }
        public long NuipComparenciente { get; set; }
        public long IdTramite { get; set; }
        public string NombreOperador { get; set; }
        public string NombreCompareciente { get; set; }

    }
}