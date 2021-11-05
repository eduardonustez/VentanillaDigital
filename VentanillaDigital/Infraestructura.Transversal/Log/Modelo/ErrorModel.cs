using Infraestructura.Transversal.Log.Enumeracion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Modelo
{
    public class ErrorModel
    {
        public string TransaccionId { get; set; }
        public string Usuario { get; set; }
        public string mensaje { get; set; }
        public string Capa { get; set; }
        public string Clase { get; set; }
        public string Metodo { get; set; }
        public string Modelo { get; set; }
        public Exception exception { get; set; }

        public ErrorModel()
        {

        }

        public ErrorModel(string guid, Exception exc, PoliticaCapaEnum capa, string clase, string metodo, object modelo,string usuario )
        {
            this.Capa = capa.ToString();
            this.TransaccionId = guid;
            this.mensaje = exc!=null? exc.Message:"";
            this.exception = exc;
            this.Clase = clase;
            this.Metodo = metodo;
            this.Usuario = usuario;
            if (modelo != null)
                this.Modelo = JsonConvert.SerializeObject(modelo);
        }
        
    }
}
