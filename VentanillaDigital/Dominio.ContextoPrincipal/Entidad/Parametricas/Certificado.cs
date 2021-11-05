using Dominio.Nucleo.Entidad;
using System.Collections.Generic;
using System;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Certificado:EntidadBase
    {
        public long CertificadoId { get; set; }
        public string UsuarioId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Datos { get; set; }
        public int Estado { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
