using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class CertificadoCreateDTO:NewRegisterDTO
    {
        public string Notaria { get; set; }
        public string DniNotarioPrincipal { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Direccion { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Cargo { get; set; }
        //public byte[] Cedula { get; set; }
        //public byte[] CComercio { get; set; }
        //public byte[] Contrato { get; set; }
    }
}
