using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dominio.Nucleo.Entidad
{
    public abstract class EntidadBase
    {
        public DateTime FechaCreacion { get; set; } 
        public DateTime FechaModificacion { get; set; } 
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool IsDeleted { get; set; }

        public void EliminarLogico()
        {
            this.IsDeleted = true;
        }
      
    }
}
