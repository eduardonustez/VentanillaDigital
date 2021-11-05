using Infraestructura.Transversal.Log.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Nucleo.Observador
{
    public abstract class Observer
    {
        public abstract void NotificarEvento(InformationModel informacionPersistida);
        public abstract void NotificarExcepcion(ErrorModel errorModelo);
    }
}
