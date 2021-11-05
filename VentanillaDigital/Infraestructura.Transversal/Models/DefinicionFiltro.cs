using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Models
{
    public class DefinicionFiltro
    {
        public List<Ordenacion> Ordenacion { get; set; }

        public int IndicePagina { get; set; }

        public int RegistrosPagina { get; set; }

        public long TotalRegistros { get; set; }

        public List<Filtro> Filtros { get; set; }
    }

    public class DefinicionFiltroSimple
    {
        public int IndicePagina { get; set; }

        public int RegistrosPagina { get; set; } = 10;
    }

    public class Ordenacion
    {
        public string ColumnaOrden { get; set; }

        public string DireccionOrden { get; set; }
        public Ordenacion()
        {

        }

        public Ordenacion(string columna,string direccion="ASC")
        {
            ColumnaOrden = columna;
            DireccionOrden = direccion;
        }
    }
    public class Filtro
    {
        public Filtro()
        {

        }
        public Filtro(string columna, string valor, string condicion = "=", string valorFin = "", string operador = "AND")
        {
            this.Columna = columna;
            this.Valor = valor;
            this.Operador = operador;
            this.Condicion = condicion;
            this.ValorFin = valorFin;
        }

        public string Operador { get; set; }
        public string Columna { get; set; }
        public string Condicion { get; set; }
        public string Valor { get; set; }
        public string ValorFin { get; set; }
    }
    public class RespuestaProcedimientoViewModel
    {
        public string Resultado { get; set; }
        public long TotalRegistros { get; set; }
        public long TotalPaginas { get; set; }
    }

    public class RespuestaServicioViewModel
    {
        public long TotalRegistros { get; set; }
        public long TotalPaginas { get; set; }
    }



}
