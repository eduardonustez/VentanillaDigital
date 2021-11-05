using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class NotarioDTO : ResponseDTO
    {
        public long NotarioId { get; set; }
        public string Nit { get; set; }
        public string Grafo { get; set; }
        public string Pin { get; set; }
        public int TipoNotario { get; set; }
        public long NotariaUsuariosId { get; set; }
    }
    public class EstadoPinFirmaDTO
    {
        public bool PinAsignado { get; set; }
        public bool FirmaRegistrada { get; set; }
        public bool CertificadoSolicitado { get; set; }
    }
    public class OpcionesConfiguracioNotarioDTO
    {
        public bool UsarSticker { get; set; }
    }
    public class SeleccionarFormatoImpresionDTO:NewRegisterDTO
    {
        public bool UsarSticker { get; set; }
    }
}
