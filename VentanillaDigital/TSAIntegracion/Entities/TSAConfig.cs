using System;
using System.Collections.Generic;
using System.Text;

namespace TSAIntegracion.Entities
{
    public class TSAConfig : ITSAConfig
    {
        public const string TSAConfigName = "TSAConfig";
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Algorithm { get; set; }
        public string CertificateSerialNumber { get; set; }
        public string CertificatePassword { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
    }

    public interface ITSAConfig
    {
        string Url { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Algorithm { get; set; }
        string CertificateSerialNumber { get; set; }
        string CertificatePassword { get; set; }
        string Location { get; set; }
        string Reason { get; set; }
    }
}
