using System;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ActaNotarialGetDTO :NewRegisterDTO 
    {
        public long TramiteId { get; set; }
    }

    public class ActaNotarialSeguraRequest
    {
        public long NotariaId { get; set; }
        public DateTime FechaTramite { get; set; }
        public string Email { get; set; }
        public string Tramiteid { get; set; }
    }
}