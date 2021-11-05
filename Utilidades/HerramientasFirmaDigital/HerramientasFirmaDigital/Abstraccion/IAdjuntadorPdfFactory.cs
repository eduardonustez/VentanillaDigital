namespace HerramientasFirmaDigital.Abstraccion
{
    public interface IAdjuntadorPdfFactory
    {
        IAdjuntadorPdf Obtener(byte[] documento);
    }
}