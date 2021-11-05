using Dominio.Nucleo.Entidad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    [Table("ObtenerNotarias", Schema = "Parametricas")]
    public class NotariaCliente: EntidadBase
    {
        [Key]
        public long NotariaId
        {
            get;
            set;
        }

        public string Nombre
        {
            get;
            set;
        }

        public string Departamento
        {
            get;
            set;
        }

        public string Ciudad
        {
            get;
            set;
        }
    }
}