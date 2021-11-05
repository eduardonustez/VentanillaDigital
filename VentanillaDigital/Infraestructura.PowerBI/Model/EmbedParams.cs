using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.PowerBI
{
    public class EmbedParams
    {
        public string Type { get; set; }
        public List<EmbedReport> EmbedReport { get; set; }
        public EmbedToken EmbedToken { get; set; }
        public Guid Filter { get; set; }
    }
}
