using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PortalCliente.Data
{
    public class Compareciente : PersonaInfo
    {
        public DateTime FechaCreacion { get; set; }
        public string NombresRNEC { get; set; }
        public string ApellidosRNEC { get; set; }
        public string NombreRNEC { get => $"{NombresRNEC} {ApellidosRNEC}"; }
        public string Foto { get; set; }
        public string Firma { get; set; }
        public string Documento { get; set; }
        public string TransaccionRNECId { get; set; }
        public bool FotoOk { get; set; }
        public bool EscaneoOk { get; set; }
        public bool AtdpOk { get; set; }
        public bool HuellasOk { get; set; }
        public string ResulHuellas { get; set; }
        public string ResulHuellasDetalle { get; set; }
        public bool TramiteSinBiometria { get; set; }
        public string MotivoSinBiometria { get; set; }
        public ExcepcionHuella Excepciones { get; set; }
        public Tramite Tramite { get; set; }
        public string NombresDigitados { get => base.Nombres; set => base.Nombres = value; }
        public string ApellidosDigitados { get => base.Apellidos; set => base.Apellidos = value; }
        public string NombreCompletoDigitado { get => $"{NombresDigitados} {ApellidosDigitados}"; }
        public int? ComparecienteActualPos { get; set; }

        public string DedoUnoResultado { get; set; }
        public string DedoDosResultado { get; set; }

        public int ScoreDedoUnoResultado { get; set; }
        public int ScoreDedoDosResultado { get; set; }
        public override string Nombres
        {
            get => string.IsNullOrWhiteSpace(NombresRNEC) ? base.Nombres : NombresRNEC;
            set => base.Nombres = value;
        }
        public override string Apellidos
        {
            get => string.IsNullOrWhiteSpace(ApellidosRNEC) ? base.Apellidos : ApellidosRNEC;
            set => base.Apellidos = value;
        }

        public override bool IsValid(bool terminando = false)
        {
            bool valid = true;
            valid &= Tramite.Comparecientes.All(x => x.NumeroIdentificacion != NumeroIdentificacion || x == this);

            if (terminando)
            {
                valid &= HuellasOk || TramiteSinBiometria;
            }
            return valid && base.IsValid(terminando);
        }

        public static Compareciente ObtenerNuevoCompareciente()
        {
            var ret = new Compareciente();
            return ret;
        }

    }
}
