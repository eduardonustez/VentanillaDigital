namespace ApiGateway.Contratos.Models.Configuraciones
{
    public class ScannerConfigModel
    {
        public bool UsarScanner { get; set; }
        public OpcionesScanner Opciones { get; set; }
    }

    public class OpcionesScanner
    {
        public string NombreDispositivo { get; set; }
        public int Dpi { get; set; }
    }
}
