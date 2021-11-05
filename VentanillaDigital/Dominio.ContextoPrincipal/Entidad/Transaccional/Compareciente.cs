using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class Compareciente : EntidadBase
    {
        public long ComparecienteId { get; set; }
        public long TramiteId { get; set; }
        public long FotoId { get; set; }
        public long FirmaId { get; set; }
        public long ImagenDocumentoId { get; set; }
        public string PeticionRNEC { get; set; }
        public long PersonaId { get; set; }
        public bool TramiteSinBiometria { get; set; }
        public string MotivoSinBiometria { get; set; }
        public int? Posicion { get; set; }
        public bool? HitDedo1 { get; set; }
        public bool? HitDedo2 { get; set; }
        public string? Vigencia { get; set; }
        public string NombreDigitado { get; set; }
        public virtual MetadataArchivo Foto { get; set; }
        public virtual MetadataArchivo Firma { get; set; }
        public virtual MetadataArchivo ImagenDocumento { get; set; }
        public virtual ExcepcionHuella ExcepcionHuella { get; set; }
        public virtual Persona Persona { get; set; }

    }
}
