namespace ApiGateway.Models.Transaccional
{
    public class ActaNotarialModel
    {
        public string Archivo { get; set; }
        public bool Autorizada { get; set; }
        public bool Rechazada { get; set; }
    }
    public class FirmaActaNotarialModel
    {
        public string Archivo { get; set; }
        public bool EsError { get; set; }
        public int CodigoResultado { get; set; }
    }

    public class TramiteRechazadoModel
    {
        public string Pin { get; set; }
        public string MotivoRechazo { get; set; }
        public long TramiteId { get; set; }
    }

    public class ComparecienteOmitidoModel
    {
        public string MotivoOmision { get; set; }
        public long TramiteId { get; set; }
    }

    public class TramiteRechazadoReturnModel
    {
        public int CodigoResultado { get; private set; }
        public string Estado { get; set; }
        public bool EsError { get; set; }
    }


}
