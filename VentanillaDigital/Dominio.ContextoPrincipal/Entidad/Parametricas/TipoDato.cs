using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class TipoDato
    {
        public TipoDatoId TipoDatoId { get; set; }
        public string Nombre { get; set; }
    }
    public enum TipoDatoId : int
    {
        Direccion = 1,
        NumeroCelular=2,
        Email=3
    }

}
