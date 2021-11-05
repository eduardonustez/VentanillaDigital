using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Models.Transaccional
{
    public class TramitePendienteAutorizacionModel
    {
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<ListaTramitePendienteReturnDTO> Pendientes { get; set; }
    }
    public class ListaTramitePendienteReturnDTO
    {
        public long Id { get; set; }
        public string TipoTramite { get; set; }
        public DateTime Fecha { get; set; }
        public int Comparecientes { get; set; }
        public string DocCompareciente { get; set; }
        public string Estado { get; set; }
        public string UsuarioOperador { get; set; }
        public string NombreOperador { get; set; }
        public string MotivoRechazo { get; set; }
    }
}
