using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Rest
{
    public class EnvioArchivoFirmadoNotarioAutorizadoModel
    {
        public string notaryProcedureID { get; set; }
        public int state { get; set; }
        public List<FilesModel> Files { get; set; }
    }

    public class FilesModel
    {
        public string Format { get; set; }
        public string Base64 { get; set; }
        public string Name { get; set; }
    }
}
