using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class RFDPostResponse
    {
        public string UserId { get; set; }
        public string TrackingIds { get; set; }
        public string TimeStamp { get; set; }
        public string Mensaje { get; set; }
        public bool Success { get; set; }
        public string Errors { get; set; }
    }
}
