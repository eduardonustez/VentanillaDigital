using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Rest
{
    public class CreatePaymentLinkModel
    {
        //public int OrderAmount { get; set; }
        //public int OrderTax { get; set; }
        //public string ItemName { get; set; }
        //public long ItemReference { get; set; }
        //public long ServiceId { get; set; }
        //public string DocumentBase64 { get; set; }
        public long OrderAmount { get; set; }
        public long OrderTax { get; set; }
        public string Concept { get; set; }
        public string Email { get; set; }
        public string Cuandi { get; set; }
        public string NombreDestinatario { get; set; }
        public string DocumentBase64 { get; set; }
        public string Document2Base64 { get; set; }
        public string Document3Base64 { get; set; }
        public long TransaccionId { get; set; }
    }
}
