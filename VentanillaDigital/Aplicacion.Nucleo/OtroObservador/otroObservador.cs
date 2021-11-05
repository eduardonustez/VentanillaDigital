using Aplicacion.Nucleo.Base;
using Aplicacion.Nucleo.Observador;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Aplicacion.Nucleo
{
    public class otroObservador:Observer
    {
        public override void NotificarEvento(InformationModel informacionPersitida)
        {
           //do something
          
        }
        public override void NotificarExcepcion(ErrorModel errorModelo)
        {
            string docPath = @"d:\";

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "NotificacionErorSDC.txt")))
            {
                 outputFile.WriteAsync(DateTime.Now + ": " + errorModelo.exception.Message + "\n");
            }

        }
    }
}
