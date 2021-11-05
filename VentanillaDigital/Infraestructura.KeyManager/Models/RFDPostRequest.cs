using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class RFDPostRequest
    {
        public RFDContacto Contacto { get; set; }
        public string Tipo { get; set; }
        public RFDDatos Datos { get; set; }
        public RFDTermsAndConditions Tyc { get; set; }
        public List<RFDFileEvidence> FileEvidences { get; set; }
        public string Description { get; set; }

        public RFDPostRequest( string tipo, string Description)
        {
            this.Tipo = tipo;
            this.Description = Description;
            FileEvidences = new List<RFDFileEvidence>();
        }
        public void SetContacto(string correo,string telefono)
        {
            Contacto = new RFDContacto()
            {
                Correo = correo,
                Telefono = telefono
            };
        }
        public void SetDatos(string razonSocial,string nit,string nombresRepresentanteLegal,
                string apellidosRepresentanteLegal,string tipoDocumentoRepresentanteLegal,
                string numeroDocumentoRepresentanteLegal,string departamento,string ciudad,
                string pin,string notary,string notaryAddress,string position)
        {
            this.Datos = new RFDDatos()
            {
                RazonSocial = $"{apellidosRepresentanteLegal} {nombresRepresentanteLegal}",
                Nit = nit,
                NombresRepresentanteLegal = nombresRepresentanteLegal,
                ApellidosRepresentanteLegal = apellidosRepresentanteLegal,
                TipoDocumentoRepresentanteLegal = tipoDocumentoRepresentanteLegal,
                NumeroDocumentoRepresentanteLegal = numeroDocumentoRepresentanteLegal,
                Departamento = departamento,
                Ciudad = ciudad,
                Pin = pin,
                //Notary = $"{apellidosRepresentanteLegal} {nombresRepresentanteLegal}",
                Notary = nombresRepresentanteLegal,
                NotaryAddress = notaryAddress,
                Position = position
            };
        }
        public void SetTyc(bool aceptar,string tyc)
        {
            this.Tyc = new RFDTermsAndConditions()
            {
                Aceptar = aceptar,
                Tyc = tyc
            };
        }
        public void AddFileEvidence(string fileName,string base64,string fileType)
        {
            this.FileEvidences.Add(new RFDFileEvidence()
            {
                FileName = fileName,
                Base64 = base64,
                FileType = fileType
            }); 
        }
    }
    public class RFDContacto
    {
        public string Correo { get; set; }
        public string Telefono { get; set; }
    }
    public class RFDDatos
    {
        public string RazonSocial { get; set; }
        public string Nit { get; set; }
        public string NombresRepresentanteLegal { get; set; }
        public string ApellidosRepresentanteLegal { get; set; }
        public string TipoDocumentoRepresentanteLegal { get; set; }
        public string NumeroDocumentoRepresentanteLegal { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Pin { get; set; }
        public string Notary { get; set; }
        public string NotaryAddress { get; set; }  
        public string Position { get; set; }
    }
    public class RFDTermsAndConditions {
        public bool Aceptar { get; set; }
        public string Tyc { get; set; }
    }
    public class RFDFileEvidence
    {
        public string FileName { get; set; }
        public string Base64 { get; set; }
        public string FileType { get; set; }
    }
}
