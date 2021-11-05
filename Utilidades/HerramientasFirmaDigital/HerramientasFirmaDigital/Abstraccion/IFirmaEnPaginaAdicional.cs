namespace HerramientasFirmaDigital.Abstraccion
{
    public interface IFirmaEnPaginaAdicional
    {
        byte[] FirmarEnNuevaPagina(byte[] documento, DatosFirma datosFirma);
    }
}