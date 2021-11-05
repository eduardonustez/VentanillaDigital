using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models
{
    public class ComparecienteReturnDTO
    {
        public long ComparecienteId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Foto { get; set; }
        public string Firma { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string MotivoSinBiometria { get; set; }
        public bool TramiteSinBiometria { get; set; }

        public TipoIdentificacionModel TipoIdentificacion { get; set; }
        public int Posicion { get; set; }

    }
}
