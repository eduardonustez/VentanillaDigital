using Microsoft.AspNetCore.Components;
using System;

namespace PortalCliente.Pages.TramitePages
{
    public partial class Details_Multilevel_result
    {
        [Parameter] public string JsonData { get; set; }

        public Newtonsoft.Json.Linq.JObject JsonDataObject { get; set; }

        protected override void OnInitialized()
        {
            JsonDataObject = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonData) as Newtonsoft.Json.Linq.JObject;
        }

        MarkupString Raw(string value)
        {
            return (MarkupString)value;
        }
    }
}
