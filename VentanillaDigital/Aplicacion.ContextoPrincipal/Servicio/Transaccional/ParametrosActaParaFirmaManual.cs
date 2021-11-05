using Aplicacion.ContextoPrincipal.Enums;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Aplicacion.TareasAutomaticas.Enums;
using Aplicacion.TareasAutomaticas.Modelo.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using GenericExtensions;
using HtmlToPdf.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio.Transaccional
{
    public static class ParametrosActaParaFirmaManual
    {
        public static async Task<PlantillaParametros> ObtenerParametros(long CodigoTipoTramite,bool UsarSticker, ActaCreateToPlantilla actaCreate
            ,List<ComparecienteCreate> comparecientes, string DataAdicional, string textoFolio, bool SabeFirmar)
        {
            

            PlantillaParametros plantillaParametros = new PlantillaParametros();
            var keyvalues = actaCreate.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(actaCreate, null));

            var parametros = keyvalues.Select(kv => new Parametro()
            {
                NombreCampo = kv.Key,
                Valor = kv.Value
            }).ToList();

           

            var datosAdicionales = JsonConvert.DeserializeObject<Dictionary<string, string>>(DataAdicional);

            if (datosAdicionales != null)
            {
                foreach (var kv in datosAdicionales)
                {
                    Parametro parametro = new Parametro() { NombreCampo = kv.Key, Valor = kv.Value };
                    parametros.Add(parametro);
                }
                
                Parametro parametroTextoFolio = new Parametro() { NombreCampo = "TextoFolio", Valor = textoFolio };
                parametros.Add(parametroTextoFolio);
            }
            else
            {
                Parametro parametroTextoFolio = new Parametro() { NombreCampo = "TextoFolio", Valor = string.Empty };
                parametros.Add(parametroTextoFolio);
            }

            Parametro parametroTextoBiometriaPlural;
            if (!UsarSticker && comparecientes.Count > 1 && comparecientes.Count(x => x.TramiteSinBiometria == "False") == comparecientes.Count)
            {
                string textoBiometriaPlural = "Conforme al Artículo 18 del Decreto - Ley 019 de 2012, los comparecientes fueron identificados mediante cotejo biométrico en línea de su huella dactilar con la información biográfica y biométrica de la base de datos de la Registraduría Nacional del Estado Civil.";
                parametroTextoBiometriaPlural = new Parametro() { NombreCampo = "TextoBiometriaPlural", Valor = textoBiometriaPlural };
                parametros.Add(parametroTextoBiometriaPlural);

                foreach(var compareciente in comparecientes)
                {
                    compareciente.TextoBiometria = null;
                }

            }
            else
            {
                parametroTextoBiometriaPlural = new Parametro() { NombreCampo = "TextoBiometriaPlural", Valor = "" };
                parametros.Add(parametroTextoBiometriaPlural);
            }

            plantillaParametros.Parametros = parametros;
            if (CodigoTipoTramite == (long)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoConFirmaARuego ||
                (!SabeFirmar && CodigoTipoTramite == (long)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoDePersonaInvidente)
                )
            {
                var comparecientesFirmaARuego = await ObtenerComparecientesFirmaARuego(comparecientes,!string.IsNullOrWhiteSpace(parametroTextoBiometriaPlural.Valor));
                plantillaParametros.Parametros = plantillaParametros.Parametros.Union(comparecientesFirmaARuego);
                //plantillaParametros.Comparecientes = null;
            }
            plantillaParametros.Comparecientes = await ObtenerParametrosComparecientes(comparecientes);

            return plantillaParametros;
        }
        private static async Task<List<IEnumerable<Parametro>>> ObtenerParametrosComparecientes(List<ComparecienteCreate> ListaComparecientes)
        {

            var parametrosTodosCompa =
                ListaComparecientes.Select(c =>
                {
                    var kvcomp = c.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(c, null));
                    return kvcomp.Select(kv => new Parametro()
                    {
                        NombreCampo = kv.Key,
                        Valor = kv.Value
                    });
                }).ToList();

            return parametrosTodosCompa;
        }

        private static async Task<List<Parametro>> ObtenerComparecientesFirmaARuego(List<ComparecienteCreate> ListaComparecientes,bool usarTextoBiometriaPlural)
        {
            List<Parametro> ComparecientesFirmaARuego = new List<Parametro>();
            foreach (var ct in ListaComparecientes)
            {
                if (ct.Posicion == "1")
                {
                    ActaNotarialComparecientesValoresDTO comp = new ActaNotarialComparecientesValoresDTO()
                    {
                        NombreTipoDocumento = ct.NombreTipoDocumento,
                        NombreCompareciente = $"{ct.Nombres} {ct.Apellidos}",
                        NUIPCompareciente = ct.NUIPCompareciente,
                        FotoCompareciente = ct.FotoCompareciente,
                        FirmaCompareciente = ct.FirmaCompareciente,
                        NUT = ct.NUT,
                        FechaCompletaNumeros = ct.FechaCompletaNumeros,
                        HoraCompletaNumeros = ct.HoraCompletaNumeros,
                        TextoBiometria = usarTextoBiometriaPlural?null: ct.TextoBiometria
                    };
                    var keyvalues = comp.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(comp, null));

                    foreach (KeyValuePair<string, string> kv in keyvalues)
                    {
                        ComparecientesFirmaARuego.Add(new Parametro(kv.Key, kv.Value, ""));
                    }
                }
                if (ct.Posicion == "2")
                {
                    ActaNotarialRogadoValoresDTO comp = new ActaNotarialRogadoValoresDTO()
                    {
                        NombreTipoDocumentoRogado = ct.NombreTipoDocumento,
                        NombreRogado = $"{ct.Nombres} {ct.Apellidos}",
                        NUIPRogado = ct.NUIPCompareciente,
                        FotoRogado = ct.FotoCompareciente,
                        FirmaRogado = ct.FirmaCompareciente,
                        NUTRogado = ct.NUT,
                        FechaRogado = ct.FechaCompletaNumeros,
                        HoraRogado = ct.HoraCompletaNumeros,
                        TextoBiometriaRogado = usarTextoBiometriaPlural ? null : ct.TextoBiometria
                    };
                    var keyvalues = comp.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(comp, null));

                    foreach (KeyValuePair<string, string> kv in keyvalues)
                    {
                        ComparecientesFirmaARuego.Add(new Parametro(kv.Key, kv.Value, ""));
                    }
                }

            }

            return ComparecientesFirmaARuego;

        }
    }
}
