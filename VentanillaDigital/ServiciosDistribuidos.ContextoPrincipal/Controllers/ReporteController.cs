using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Infraestructura.PowerBI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReporteController : BaseController
    {
        private IReporteService p_reporteService { get; }
        private INotariaServicio _notariaServicio { get; }
        public ReporteController(IReporteService reporteService
            , INotariaServicio notariaServicio)
        {
            p_reporteService = reporteService;
            _notariaServicio = notariaServicio;
        }

        [HttpPost]
        [Route("ReporteOperacionalDiario")]
        public async Task<string> ReporteOperacionalDiario(ReporteRequestDTO reporte)
        {            
            Guid guidNotaria = await _notariaServicio.ObtenerConvenioRNEC(reporte.NotariaId).ConfigureAwait(false);
            var reportes = p_reporteService.ObtenerReporteEmbed(reporte.TipoReporte, guidNotaria);
            return JsonSerializer.Serialize(reportes);
        }
    }
}
