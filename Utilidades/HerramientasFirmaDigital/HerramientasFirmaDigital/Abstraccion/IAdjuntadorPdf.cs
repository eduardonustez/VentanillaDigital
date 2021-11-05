
namespace HerramientasFirmaDigital.Abstraccion
{
    public interface IAdjuntadorPdf
    {
        IAdjuntadorPdf Adjuntar(byte[] nuevoDocumento);
        byte[] Generar();
    }
}