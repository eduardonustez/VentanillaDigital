using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Job
{
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    public class GenerarDocumentos : CronJobService
    {
        private readonly ILogger<GenerarDocumentos> _logger;

        public GenerarDocumentos(IScheduleConfig<GenerarDocumentos> config, ILogger<GenerarDocumentos> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GenerarDocumentos starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} GenerarDocumentos is working.");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GenerarDocumentos is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
