using System;

namespace ServiciosDistribuidos.TareasAutomaticas.Jobs
{
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
        public int Delay { get; set; }
    }
}
