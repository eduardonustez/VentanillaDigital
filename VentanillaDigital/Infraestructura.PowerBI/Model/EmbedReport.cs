using System;

namespace Infraestructura.PowerBI
{
    public class EmbedReport
    {
        public Guid ReportId { get; set; }
        public string ReportName { get; set; }
        public string EmbedUrl { get; set; }
    }
}
