namespace Infraestructura.Transversal.Models
{
    public class ComparecienteData
    {
        public long ComparecienteId { get; set; }
        public string NombreCompareciente { get; set; }
        public string NUIPCompareciente { get; set; }
        public string FotoCompareciente { get; set; }
        public string FirmaCompareciente { get; set; }
        public string NUT { get; set; }
        //public string FechaCompletaNumeros { get; set; }
        //public string HoraCompletaNumeros { get; set; }
        public bool TramiteSinBiometria { get; set; }
        public string MotivoSinBiometria { get; set; }
        public string NombreTipoDocumento { get; set; }
    }
}
