using Infraestructura.Transversal.Log.Enumeracion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Modelo
{
    public class InformationModel
    {
        public string TransaccionId { get; set; }
        public string Capa { get; set; }
        public string Clase { get; set; }
        public string Metodo { get; set; }
        public string Entidad { get; set; }
        public string EntidadId { get; set; }
        public string Datos { get; set; }
        public string Usuario { get; set; }
        public InformationModel()
        {

        }
        public InformationModel(string transaccionId
                               ,string capa,string clase,string metodo,string entidad
                                ,string entidadId
                                ,string datos,string usuario)
        {
            this.TransaccionId = transaccionId;
            this.Capa = capa;
            this.Clase = clase;
            this.Metodo = metodo;
            this.Entidad = entidad;
            this.EntidadId = entidadId;
            this.Usuario = usuario;
            this.Datos = datos;
        }
       
    }
}
