using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Certificado
{
    public class SolicitudCertificadoDto
    {
        public string Notaria { get; set; }
        public string DniNotarioPrincipal { get; set; }
        public string TipoDocumentoNotarioPrincipal { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Direccion { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Cargo { get; set; }
        public string PinFirma { get; set; }
        public byte[] Cedula { get; set; }
        public byte[] CComercio { get; set; }
        public byte[] Rut { get; set; }
        public byte[] CedulaPrincipal { get; set; }
        public byte[] Autorizacion { get; set; }
        public byte[] Contrato { get; set; }
        public byte[] Firma { get; set; }
        public bool AceptarTyc { get; set; }
        public string NombreOPeradorThomas { get; set; }
    }
}
