using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class TipoTramite:EntidadBase
    {
        public long TipoTramiteId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public long CategoriaId { get; set; }
        public long CodigoTramite { get; set; }
        public long PlantillaActaId { get; set; }
        public long? PlantillaStickerId { get; set; }
        public long? PlantillaDosStickerId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual PlantillaActa PlantillaActa { get; set; }
        public virtual PlantillaActa PlantillaSticker { get; set; }
        public virtual PlantillaActa PlantillaDosSticker { get; set; }
        public long ProductoReconoserId { get; set; }
    }
}
