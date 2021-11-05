using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Entidades;

namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Servicios.Interfaces
{
    public interface IDigitalizacionNotarialServicio
    {
        ActoNotarialProtocoloResponse ActoNotarialProtocolo(ActoNotarialProtocoloRequest request);
        ServiceInfoResponse ServiceInfo();
        CheckInResponse CheckIn(CheckInRequest request);
    }
}