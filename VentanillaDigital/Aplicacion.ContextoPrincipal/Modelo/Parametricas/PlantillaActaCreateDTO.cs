using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class PlantillaActaCreateDTO : NewRegisterDTO
    {
        public string Nombre { get; set; }
        public string Contenido { get; set; }
    }
    public class PlantillaActaEditDTO : EditRegisterDTO
    {
        public long PlantillaActaId { get; set; }
        public string Nombre { get; set; }
        public string Contenido { get; set; }
    }

    public class PlantillaActaReturnDTO 
    {
        public long PlantillaActaId { get; set; }
        public string Nombre { get; set; }
        public TipoTramiteReturnDTO[] TiposTramites { get; set; }
    }
    public class PlantillaActaFullReturnDTO
    {
        public long PlantillaActaId { get; set; }
        public string Nombre { get; set; }
        public string Contenido { get; set; }
        public TipoTramiteReturnDTO[] TiposTramites { get; set; }
    }
    public class PlantillaActaAssociateDTO : EditRegisterDTO
    {
        public long PlantillaActaId { get; set; }
        public long[] CodigosTramites { get; set; }
    }

}
