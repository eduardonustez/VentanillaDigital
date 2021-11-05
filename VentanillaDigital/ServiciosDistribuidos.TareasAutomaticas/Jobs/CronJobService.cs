using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.TareasAutomaticas.Jobs
{
    public abstract class CronJobService : IHostedService, IDisposable
    {
        #region Events Ids
        private readonly EventId ReagendamientoJOB = new EventId(1, "Reagendamiento JOB");
        private readonly EventId JOBFueraDeServicio = new EventId(2, "JOB Fuera de Servicio");
        #endregion

        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger<CronJobService> _logger;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo, ILogger<CronJobService> logger)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
            _logger = logger;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                _logger.LogInformation(ReagendamientoJOB, $"Reagendamiento del JOB {this.GetType().Name} para ejecutar en {delay.TotalSeconds} Segundos");
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Stop();  // reset timer
                    await DoWork(cancellationToken);
                    await ScheduleJob(cancellationToken);    // reschedule next

                };
                _timer.Start();
            }
            else
            {
                _logger.LogWarning(JOBFueraDeServicio, $"El Job {this.GetType().Name} ha dejado de estar en servicio.");
            }
            await Task.CompletedTask;
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);  // do the work
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
