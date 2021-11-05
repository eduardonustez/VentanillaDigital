using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.AgenteServicios.AgenteANI
{
    public class ANIResponseModel
    {
        public PersonaANI Respuesta { get; set; }
        public string CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
    }
    
    public class ANIInputModel
    {
        public int TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string CodigoAplicacion { get; set; }
    }
    public class ANIParametersModel
    {
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Aplicacion { get; set; }
    }


    public class PersonaANI
    {
        public string AnioResolucion { get; set; }
        public string CodigoErrorDatosCedula { get; set; }
        public string DepartamentoExpedicion { get; set; }
        public string EstadoCedula { get; set; }
        public string FechaExpedicion { get; set; }
        public string MunicipioExpedicion { get; set; }
        public string NUIP { get; set; }
        public string NumeroResolucion { get; set; }
        public string Particula { get; set; }
        public string PrimerApellido { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoApellido { get; set; }
        public string SegundoNombre { get; set; }
        
    }

}
