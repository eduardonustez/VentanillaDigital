using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApiGateway.Contratos.Models.Reportes
{
    public class ReporteResponse
    {
        public string Type { get; set; }
        public List<EmbedReport> EmbedReport { get; set; }
        public EmbedToken EmbedToken { get; set; }
        public Guid Filter { get; set; }
    }

    public class EmbedReport
    {
        public Guid ReportId { get; set; }
        public string ReportName { get; set; }
        public string EmbedUrl { get; set; }
    }

    public class EmbedToken
    {
        public EmbedToken() { }
        public EmbedToken(string token, Guid tokenId, DateTime expiration)
        {
            Token = token;
            TokenId = tokenId;
            Expiration = expiration;
        }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "tokenId")]
        public Guid TokenId { get; set; }

        [JsonProperty(PropertyName = "expiration")]
        public DateTime Expiration { get; set; }
    }
}
