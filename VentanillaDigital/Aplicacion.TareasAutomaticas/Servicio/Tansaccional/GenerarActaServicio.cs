using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using GenericExtensions;
using HashidsNet;
using HtmlToPdf;
using HtmlToPdf.Entidades;
using Infraestructura.Transversal.Cache;
using Infraestructura.Transversal.Encriptacion;
using GeneradorQR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Aplicacion.TareasAutomaticas.Contrato.Transaccional;
using Aplicacion.TareasAutomaticas.Modelo.Transaccional;
using Aplicacion.TareasAutomaticas.Enums;

namespace Aplicacion.TareasAutomaticas.Servicio.Tansaccional
{
    public class GenerarActaServicio : BaseServicio, IGenerarActaServicio
    {

        #region Const - Mensajes
        private const string SINTRAMITES = "Numero de tramites invalido";
        private const string CACHE_INFO_ACTA_NOTARIAL = "InfoActaNotarial_{0}_{1}";
        private const string CACHE_COMPARECIENTES = "COMPARECIENTES_{0}";
        private const string CACHE_TEXTOSINBIOMETRIA = "TEXTOSINBIOMETRIA";
        private const string CACHE_TEXTOCONBIOMETRIA = "TEXTOCONBIOMETRIA";
        private const string CACHE_NOTARIA_UBICACION = "NOTARIA_UBICACION_{0}";
        private const string CACHE_URLQR = "URLQR";
        private const string CACHE_NOTARIA_USUARIO = "NOTARIA_USUARIO_{0}";
        private const string CACHE_NOMBRETIPODOCUMENTO = "NOMBRETIPODOCUMENTO";
        private const string CACHE_IDPLANTILLASTICKERINVIDENTESABEFIRMAR = "PLANTILLASTICKERINVIDENTESABEFIRMAR";
        private const string CACHE_IDPLANTILLACARTAINVIDENTESABEFIRMAR = "PLANTILLACARTAINVIDENTESABEFIRMAR";
        private const string CACHE_PLANTILLA = "PLANTILLA_{0}";
        private const string CACHE_STICKER = "STICKER_{0}";
        private const string CACHE_STICKER_DOS = "STICKER_DOS_{0}";
        #endregion

        #region Propiedades
        private IActaNotarialRepositorio _actaNotarialRepositorio { get; }
        private ITramiteRepositorio _tramiteRepositorio { get; }
        private IComparecienteRepositorio _comparecienteRepositorio { get; }
        private INotariasUsuarioRepositorio _notariasUsuarioRepositorio { get; }
        private IPlantillaActaRepositorio _plantillaActaRepositorio { get; }
        private IHtmlToPdf _htmlToPdf { get; }
        private IGeneradorQR _generadorQR { get; }
        private INotarioRepositorio _notarioRepositorio { get; }
        private INotariaRepositorio _notariaRepositorio { get; }
        private IMemoryCache _memoryCache { get; }
        private ImplementedCache _cache { get; }
        #endregion

        #region Constructor
        public GenerarActaServicio(ITramiteRepositorio tramiteRepositorio,
            IActaNotarialRepositorio actaNotarialRepositorio,
            INotariasUsuarioRepositorio notariasUsuarioRepositorio,
            IComparecienteRepositorio comparecienteRepositorio,
            IPlantillaActaRepositorio plantillaActaRepositorio,
            INotarioRepositorio notarioRepositorio,
            INotariaRepositorio notariaRepositorio,
            IHtmlToPdf htmlToPdf,
            ImplementedCache cache,
            IMemoryCache memoryCache,
            IGeneradorQR generadorQR) : base(actaNotarialRepositorio, tramiteRepositorio
                , notariasUsuarioRepositorio, comparecienteRepositorio)
        {
            _memoryCache = memoryCache;
            _tramiteRepositorio = tramiteRepositorio;
            _actaNotarialRepositorio = actaNotarialRepositorio;
            _notariasUsuarioRepositorio = notariasUsuarioRepositorio;
            _plantillaActaRepositorio = plantillaActaRepositorio;
            _htmlToPdf = htmlToPdf;
            _cache = cache;
            _generadorQR = generadorQR;
            _comparecienteRepositorio = comparecienteRepositorio;
            _notarioRepositorio = notarioRepositorio;
            _notariaRepositorio = notariaRepositorio;
        }

        private static string FechaALetras(DateTime fecha)
        {
            string nombreDia = NumeroALetras(fecha.Day);
            nombreDia = nombreDia == "uno" ? "primero" : nombreDia;
            return $"{nombreDia} ({fecha.Day}) de {fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))} de {NumeroALetras(fecha.Year)} ({fecha.Year})";
        }

        private static string NumeroALetras(double value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "CERO";
            else if (value == 1) num2Text = "UNO";
            else if (value == 2) num2Text = "DOS";
            else if (value == 3) num2Text = "TRES";
            else if (value == 4) num2Text = "CUATRO";
            else if (value == 5) num2Text = "CINCO";
            else if (value == 6) num2Text = "SEIS";
            else if (value == 7) num2Text = "SIETE";
            else if (value == 8) num2Text = "OCHO";
            else if (value == 9) num2Text = "NUEVE";
            else if (value == 10) num2Text = "DIEZ";
            else if (value == 11) num2Text = "ONCE";
            else if (value == 12) num2Text = "DOCE";
            else if (value == 13) num2Text = "TRECE";
            else if (value == 14) num2Text = "CATORCE";
            else if (value == 15) num2Text = "QUINCE";
            else if (value < 20) num2Text = "DIECI" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "VEINTE";
            else if (value < 30) num2Text = "VEINTI" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "TREINTA";
            else if (value == 40) num2Text = "CUARENTA";
            else if (value == 50) num2Text = "CINCUENTA";
            else if (value == 60) num2Text = "SESENTA";
            else if (value == 70) num2Text = "SETENTA";
            else if (value == 80) num2Text = "OCHENTA";
            else if (value == 90) num2Text = "NOVENTA";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "CIEN";
            else if (value < 200) num2Text = "CIENTO " + NumeroALetras(value - 100);
            else if (value == 200 || value == 300 || value == 400 || value == 600 || value == 800) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "QUINIENTOS";
            else if (value == 700) num2Text = "SETECIENTOS";
            else if (value == 900) num2Text = "NOVECIENTOS";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "MIL";
            else if (value < 2000) num2Text = "MIL " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
                if (value % 1000 > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if (value - Math.Truncate(value / 1000000) * 1000000 > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "UN BILLON";
            else if (value < 2000000000000) num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if (value - Math.Truncate(value / 1000000000000) * 1000000000000 > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }

            return num2Text.ToLower();
        }

        private PlantillaParametros ObtenerParametrosActa(DatosNotaria datosNotaria,
                                                       string selloNotaria,
                                                       DatosNotario datosNotario,
                                                       string grafoNotario,
                                                       DatosTramiteActa datosTramite,
                                                       List<DatosComparecienteActa> datosComparecientes,
                                                       List<DatosImagenActa> fotos,
                                                       List<DatosImagenActa> grafos,
                                                       List<Parametrica> parametricas,
                                                       bool firmando = false)
        {
            //foreach (Compareciente ct in tramite.Comparecientes)
            //{
            //Archivo foto = (await _archivoRepositorio.Obtener(a => a.IsDeleted == false && a.MetadataArchivoId == ct.Foto.MetadataArchivoId)).FirstOrDefault();
            //ct.Foto.Archivo = foto;
            //Archivo firma = (await _archivoRepositorio.Obtener(a => a.IsDeleted == false && a.MetadataArchivoId == ct.Firma.MetadataArchivoId)).FirstOrDefault();
            //ct.Firma.Archivo = firma;
            //comparecientes.Add(ct);
            //}

            string urlQr = parametricas.FirstOrDefault(p => p.Codigo == "URLQR")?.Valor;
            string TEXTOSINBIOMETRIA = parametricas.FirstOrDefault(p => p.Codigo == "TEXTOSINBIOMETRIA")?.Valor;
            string TEXTOCONNBIOMETRIA = parametricas.FirstOrDefault(p => p.Codigo == "TEXTOCONBIOMETRIA")?.Valor;

            var hashids = new Hashids("NUTNOTARIA", 12, "abcdefghijklmnopqrstuvwxyz0123456789");
            var nut = hashids.EncodeLong(datosTramite.TramiteId);
            ActaNotarialValoresDTO infoActa = new ActaNotarialValoresDTO()
            {
                TramiteId = datosTramite.TramiteId.ToString(),
                TipoTramite = datosTramite.TipoTramite,
                Municipio = datosNotaria.Municipio,
                Departamento = datosNotaria.Departamento == null ?
                        "" : $", Departamento de {datosNotaria.Departamento}",
                FechaTramite = FechaALetras(datosTramite.FechaTramite),
                NumeroNotaria = datosNotaria.NumeroNotaria == 0 ?
                        "" : $" ({datosNotaria.NumeroNotaria})",
                NumeroNotariaEnLetra = datosNotaria.NumeroNotariaEnLetras,
                NumeroNotarioEnLetra = datosNotaria.NumeroNotaria <= 10 && datosNotario.Genero != "2" ?
                    datosNotaria.NumeroNotariaEnLetras
                        .Remove(datosNotaria.NumeroNotariaEnLetras.Length - 1) + "o"
                    : datosNotaria.NumeroNotariaEnLetras,
                CirculoNotaria = datosNotaria.CirculoNotaria,
                NombreNotario = $"{datosNotario.Nombres} {datosNotario.Apellidos}",
                UrlQr = urlQr,
                TramiteQR = _generadorQR.StringToQR(urlQr + nut),
                TipoNotario = datosNotario.TipoNotario == 1 ? "" : datosNotario.Genero == "2" ? " - Encargada" : " - Encargado",
                iddoc = datosTramite.TramiteId.ToString(),
                GeneroNotario = datosNotario.Genero == "2" ? "Notaria" : "Notario",
                FirmaNotario = firmando ? grafoNotario : "",
                SelloNotaria = firmando ? selloNotaria : "",
                NUT = nut,
                DireccionComparecencia = datosTramite.FueraDeDespacho ?
                    $"Por solicitud del interesado la presente diligencia se realiza en {datosTramite.DireccionComparecencia}."
                    : "",
                DireccionDiligencia = datosTramite.FueraDeDespacho ?
                    $", mediante diligencia realizada por solicitud del interesado para servicio domiciliario en {datosTramite.DireccionComparecencia}"
                    : ""
            };

            List<ActaNotarialComparecientesValoresDTO> listaComparecientes =
                datosComparecientes.Select(ct => new ActaNotarialComparecientesValoresDTO()
                {
                    NombreTipoDocumento = ct.NombreTipoDocumento == "Pasaporte" ? "el Pasaporte" : ct.NombreTipoDocumento,
                    NombreCompareciente = $"{ct.Nombres} {ct.Apellidos}",
                    NUIPCompareciente = ct.NUIPCompareciente,
                    FotoCompareciente = fotos.Where(f => f.MetadataArchivoId == ct.FotoId)
                                             .Select(f => f.Contenido)
                                             .FirstOrDefault(),
                    FirmaCompareciente = grafos.Where(f => f.MetadataArchivoId == ct.FirmaId)
                                             .Select(f => f.Contenido)
                                             .FirstOrDefault(),
                    NUT = ct.NUT,
                    FechaCompletaNumeros = ct.FechaCreacion.ToString("dd/MM/yyyy"),
                    HoraCompletaNumeros = ct.FechaCreacion.ToString("HH:mm:ss"),
                    TramiteSinBiometria = ct.TramiteSinBiometria.ToString(),
                    TextoBiometria = ct.TramiteSinBiometria ?
                            $"{TEXTOSINBIOMETRIA} {ct.MotivoSinBiometria}." : TEXTOCONNBIOMETRIA
                }).ToList();

            PlantillaParametros plantillaParametros = new PlantillaParametros();
            var keyvalues = infoActa.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(infoActa, null));

            var parametros = keyvalues.Select(kv => new Parametro()
            {
                NombreCampo = kv.Key,
                Valor = kv.Value
            }).ToList();

            var datosAdicionales = JsonConvert.DeserializeObject<Dictionary<string, string>>(datosTramite.DatosAdicionales);

            if (datosAdicionales != null)
            {
                foreach (var kv in datosAdicionales)
                {
                    Parametro parametro = new Parametro() { NombreCampo = kv.Key, Valor = kv.Value };
                    if (parametro.NombreCampo == "TarjetaProfesional")
                        parametro.Valor = !string.IsNullOrWhiteSpace(parametro.Valor) ? $" y la T.P. # {parametro.Valor}" : "";
                    parametros.Add(parametro);
                }

                string textoFolio = CrearTextoFolio(datosTramite.TipoTramiteCodigo, datosAdicionales);
                Parametro parametroTextoFolio = new Parametro() { NombreCampo = "TextoFolio", Valor = textoFolio };
                parametros.Add(parametroTextoFolio);
            }
            else
            {
                Parametro parametroTextoFolio = new Parametro() { NombreCampo = "TextoFolio", Valor = string.Empty };
                parametros.Add(parametroTextoFolio);
            }

            var parametrosTodosCompa =
                listaComparecientes.Select(c =>
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
            bool SabeFirmar = datosAdicionales != null && datosAdicionales.ContainsKey("SabeFirmar") ? Convert.ToBoolean(datosAdicionales["SabeFirmar"]) : true;
            Parametro parametroTextoBiometriaPlural;
            if (!datosTramite.UsarSticker && listaComparecientes.Count > 1 && listaComparecientes.Count(x => x.TramiteSinBiometria == "False") == listaComparecientes.Count)
            {
                string textoBiometriaPlural = "Conforme al Artículo 18 del Decreto - Ley 019 de 2012, los comparecientes fueron identificados mediante cotejo biométrico en línea de su huella dactilar con la información biográfica y biométrica de la base de datos de la Registraduría Nacional del Estado Civil.";
                parametroTextoBiometriaPlural = new Parametro() { NombreCampo = "TextoBiometriaPlural", Valor = textoBiometriaPlural };
                parametros.Add(parametroTextoBiometriaPlural);

                for (int i = 0; i < parametrosTodosCompa.Count; i++)
                {
                    parametrosTodosCompa[i] = parametrosTodosCompa[i].Select(x => new Parametro
                    {
                        NombreCampo = x.NombreCampo,
                        Valor = x.NombreCampo == "TextoBiometria" ? null : x.Valor
                    }).ToList();
                }
            }
            else
            {
                parametroTextoBiometriaPlural = new Parametro() { NombreCampo = "TextoBiometriaPlural", Valor = "" };
                parametros.Add(parametroTextoBiometriaPlural);
            }

            plantillaParametros.Parametros = parametros;
            plantillaParametros.Comparecientes = parametrosTodosCompa;

            if (SabeFirmar && datosTramite.TipoTramiteCodigo == (long)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoDePersonaInvidente)
            {
                string IDPLANTILLAINVIDENTESABEFIRMAR = "";
                if (datosTramite.UsarSticker)
                    IDPLANTILLAINVIDENTESABEFIRMAR = parametricas.FirstOrDefault(p => p.Codigo == "IDPLANTILLASTICKERINVIDENTESABEFIRMAR")?.Valor;
                else
                    IDPLANTILLAINVIDENTESABEFIRMAR = parametricas.FirstOrDefault(p => p.Codigo == "IDPLANTILLACARTAINVIDENTESABEFIRMAR")?.Valor;
                var plantilla = _plantillaActaRepositorio.Obtener(Convert.ToInt64(IDPLANTILLAINVIDENTESABEFIRMAR));
                plantillaParametros.Plantilla = plantilla.Contenido;
                plantillaParametros.Plantilla2 = null;
            }
            else
            {
                plantillaParametros.Plantilla = datosTramite.Plantilla;
                plantillaParametros.Plantilla2 = datosTramite.Plantilla2;
            }
            return plantillaParametros;

        }

        private string CrearTextoFolio(long TipoTramiteCodigo, Dictionary<string, string> datosAdicionales)
        {
            string ActoNotarial = "";
            string Firmantes = "";
            string NumeroReferencia = "";
            string DetalleDeActo = "";
            string FechaTramite = "";
            string Destinatario = "";

            foreach (var kv in datosAdicionales)
            {
                if (kv.Key == "ActoNotarial") ActoNotarial = kv.Value;
                if (kv.Key == "Firmantes") Firmantes = kv.Value;
                if (kv.Key == "NumeroReferencia") NumeroReferencia = kv.Value;
                if (kv.Key == "DetalleDeActo") DetalleDeActo = kv.Value;
                if (kv.Key == "Destinatario") Destinatario = kv.Value;
                if (kv.Key == "FechaTramite") FechaTramite = kv.Value;
            }

            string textoActo = "Este folio se vincula al documento de " + ActoNotarial + " signado por el compareciente";
            string textoFirmantes = ", en el que aparecen como partes " + Firmantes;
            string textoReferencia = " con número de referencia " + NumeroReferencia + " del día " + (!string.IsNullOrEmpty(FechaTramite) ? FechaALetras(Convert.ToDateTime(FechaTramite)) : ""); //NumeroReferencia es unico para AUTENTICACIÓN BIOMÉTRICA PARA ESCRITURA PÚBLICA
            string textoDetalles = ", sobre: " + DetalleDeActo;
            string parrafo = "";

            if (TipoTramiteCodigo == (long)EnumTipoTramite.AutenticacionBiometricaParaDeclaracionExtraProceso)
            {
                if (!string.IsNullOrEmpty(ActoNotarial) || !string.IsNullOrEmpty(Destinatario))
                {
                    if (!string.IsNullOrEmpty(ActoNotarial))
                    {
                        parrafo += "Esta acta, forma parte de la declaración extra-proceso " + ActoNotarial;
                    }
                    else
                    {
                        parrafo += "Esta acta, forma parte de la declaración extra-proceso";
                    }
                    if (!string.IsNullOrEmpty(Destinatario))
                    {
                        parrafo += ", rendida por el compareciente con destino a: " + Destinatario;
                    }
                    parrafo += ".";
                }
            }
            else if (!string.IsNullOrEmpty(ActoNotarial) || !string.IsNullOrEmpty(Firmantes)
                || !string.IsNullOrEmpty(NumeroReferencia) || !string.IsNullOrEmpty(DetalleDeActo))
            {
                if (!string.IsNullOrEmpty(ActoNotarial))
                {
                    parrafo += textoActo;
                }
                else
                {
                    parrafo += "Este folio se asocia al documento";
                }
                if (!string.IsNullOrEmpty(Firmantes))
                {
                    parrafo += textoFirmantes;
                }
                if (!string.IsNullOrEmpty(NumeroReferencia))
                {
                    parrafo += textoReferencia; //Esto solo ocurre para la AUTENTICACIÓN BIOMÉTRICA PARA ESCRITURA PÚBLICA
                }
                if (!string.IsNullOrEmpty(DetalleDeActo))
                {
                    parrafo += textoDetalles;
                }
                parrafo += ".";
            }

            return parrafo;
        }

        private async Task<List<Parametro>> ObtenerComparecientesFirmaARuego(List<DatosComparecienteActa> datosComparecienteActa,
            List<DatosImagenActa> fotos, List<DatosImagenActa> grafos, List<Parametrica> parametricas, bool TextoBiometriaPlural)
        {
            string TEXTOSINBIOMETRIA = parametricas.FirstOrDefault(p => p.Codigo == "TEXTOSINBIOMETRIA")?.Valor;
            string TEXTOCONNBIOMETRIA = parametricas.FirstOrDefault(p => p.Codigo == "TEXTOCONBIOMETRIA")?.Valor;
            List<Parametro> ComparecientesFirmaARuego = new List<Parametro>();
            foreach (DatosComparecienteActa ct in datosComparecienteActa)
            {
                if (ct.Posicion == 1)
                {
                    ActaNotarialComparecientesValoresDTO comp = new ActaNotarialComparecientesValoresDTO()
                    {
                        NombreTipoDocumento = ct.NombreTipoDocumento,
                        NombreCompareciente = $"{ct.Nombres} {ct.Apellidos}",
                        NUIPCompareciente = ct.NUIPCompareciente,
                        FotoCompareciente = fotos.Where(f => f.MetadataArchivoId == ct.FotoId)
                                             .Select(f => f.Contenido)
                                             .FirstOrDefault(),
                        FirmaCompareciente = grafos.Where(f => f.MetadataArchivoId == ct.FirmaId)
                                             .Select(f => f.Contenido)
                                             .FirstOrDefault(),
                        NUT = ct.NUT,
                        FechaCompletaNumeros = ct.FechaCreacion.ToString("dd/MM/yyyy"),
                        HoraCompletaNumeros = ct.FechaCreacion.ToString("HH:mm:ss"),
                        TramiteSinBiometria = ct.TramiteSinBiometria.ToString(),
                        TextoBiometria = ct.TramiteSinBiometria ?
                             $"{TEXTOSINBIOMETRIA} {ct.MotivoSinBiometria}." : TEXTOCONNBIOMETRIA
                    };
                    if (TextoBiometriaPlural)
                    {
                        comp.TextoBiometria = null;
                    }
                    var keyvalues = comp.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(comp, null));

                    foreach (KeyValuePair<string, string> kv in keyvalues)
                    {
                        ComparecientesFirmaARuego.Add(new Parametro(kv.Key, kv.Value, ""));
                    }
                }
                if (ct.Posicion == 2)
                {
                    ActaNotarialRogadoValoresDTO comp = new ActaNotarialRogadoValoresDTO()
                    {
                        NombreTipoDocumentoRogado = ct.NombreTipoDocumento,
                        NombreRogado = $"{ct.Nombres} {ct.Apellidos}",
                        NUIPRogado = ct.NUIPCompareciente,
                        FotoRogado = fotos.Where(f => f.MetadataArchivoId == ct.FotoId)
                                             .Select(f => f.Contenido)
                                             .FirstOrDefault(),
                        FirmaRogado = grafos.Where(f => f.MetadataArchivoId == ct.FirmaId)
                                             .Select(f => f.Contenido)
                                             .FirstOrDefault(),
                        NUTRogado = ct.NUT,
                        FechaRogado = ct.FechaCreacion.ToString("dd/MM/yyyy"),
                        HoraRogado = ct.FechaCreacion.ToString("HH:mm:ss"),
                        TextoBiometriaRogado = ct.TramiteSinBiometria ?
                             $"{TEXTOSINBIOMETRIA} {ct.MotivoSinBiometria}." : TEXTOCONNBIOMETRIA
                    };
                    if (TextoBiometriaPlural)
                    {
                        comp.TextoBiometriaRogado = null;
                    }
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

        #endregion


        public async Task<(string archivo, string tiempos)> GenerarArchivoPrevioAsync(
            long tramiteId,
            bool usarSticker = false,
            string notarioUser = null,
            string grafo = "",
            string sello = "",
            bool esFirmaManual = false,
            DatosActaFirmaManual datosActaFirmaManual = null)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tramite = await _tramiteRepositorio
                .ObtenerTodo()
                .Where(m => m.TramiteId == tramiteId)
                .Select(m => new
                {
                    m.TipoTramite.CodigoTramite,
                    Sticker = m.TipoTramite.PlantillaSticker.Contenido,
                    Sticker2 = m.TipoTramite.PlantillaDosSticker == null ? null :
                                m.TipoTramite.PlantillaDosSticker.Contenido,
                    m.NotariaId,
                    Plantilla = m.TipoTramite.PlantillaActa.Contenido,
                    m.TipoTramiteId,
                    m.Fecha,
                    m.DatosAdicionales
                })
                .FirstOrDefaultAsync();

            var tTramite = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            if (tramite == null) throw new Exception("Trámite no encontrado");

            string documentoPrevio;
            (PlantillaParametros, string) parametrosRes;

            var textoSinBiometria = await _cache.GetFromCache(CACHE_TEXTOSINBIOMETRIA, () => _actaNotarialRepositorio.ObtenerParametrica("TEXTOSINBIOMETRIA"));
            var textoConBiometria = await _cache.GetFromCache(CACHE_TEXTOCONBIOMETRIA, () => _actaNotarialRepositorio.ObtenerParametrica("TEXTOCONBIOMETRIA"));
            var urlQr = await _cache.GetFromCache(CACHE_URLQR, () => _actaNotarialRepositorio.ObtenerParametrica("URLQR"));
            var IDPLANTILLASTICKERINVIDENTESABEFIRMAR = await _cache.GetFromCache(CACHE_IDPLANTILLASTICKERINVIDENTESABEFIRMAR, () => _actaNotarialRepositorio.ObtenerParametrica("IDPLANTILLASTICKERINVIDENTESABEFIRMAR"));
            var IDPLANTILLACARTAINVIDENTESABEFIRMAR = await _cache.GetFromCache(CACHE_IDPLANTILLACARTAINVIDENTESABEFIRMAR, () => _actaNotarialRepositorio.ObtenerParametrica("IDPLANTILLACARTAINVIDENTESABEFIRMAR"));

            var parametricas = new List<Parametrica>() { textoSinBiometria, textoConBiometria, urlQr, IDPLANTILLASTICKERINVIDENTESABEFIRMAR, IDPLANTILLACARTAINVIDENTESABEFIRMAR };

            DatosTramiteActa datosTramite;
            List<DatosComparecienteActa> datosComparecientes;
            List<DatosImagenActa> fotos;
            List<DatosImagenActa> grafos;
            if (esFirmaManual)
            {
                datosTramite = datosActaFirmaManual.datosTramiteActa;
                datosTramite.Plantilla = datosTramite.UsarSticker ? tramite?.Sticker : tramite.Plantilla;
                datosTramite.Plantilla2 = datosTramite.UsarSticker ? tramite?.Sticker2 : null;
                datosComparecientes = datosActaFirmaManual.comparecientesFirmaManual;
                fotos = datosActaFirmaManual.fotosFirmaManual;
                grafos = datosActaFirmaManual.grafosFirmaManual;
            }
            else
            {
                (datosTramite, datosComparecientes, fotos, grafos) = await ObtenerTramiteActa(tramiteId,
                usarSticker);
            }

            var tTramiteActa = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            var datosAdicionales = JsonConvert.DeserializeObject<Dictionary<string, string>>(datosTramite.DatosAdicionales);
            bool SabeFirmar = false;
            if (datosAdicionales != null)
            {
                SabeFirmar = datosAdicionales.ContainsKey("SabeFirmar") ? Convert.ToBoolean(datosAdicionales["SabeFirmar"]) : false;
            }

            if (!string.IsNullOrEmpty(notarioUser))
            {
                DatosNotaria datosNotaria;
                DatosNotario datosNotario;
                string selloNotaria = "";
                string grafoNotario = "";
                if (esFirmaManual)
                {
                    datosNotaria = datosActaFirmaManual.datosNotaria;
                    datosNotario = datosActaFirmaManual.datosNotario;
                }
                else
                {
                    (datosNotaria, selloNotaria, datosNotario, grafoNotario) =
                         await _notariasUsuarioRepositorio.ObtenerNotariaUsuarioActa(notarioUser);
                }

                var tNotariaUsuarioActa = stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();

                var parametros = ObtenerParametrosActa(datosNotaria,
                                                        selloNotaria,
                                                        datosNotario,
                                                        grafoNotario,
                                                        datosTramite,
                                                        datosComparecientes,
                                                        fotos,
                                                        grafos,
                                                        parametricas);

                var tParametrosActa = stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();

                if (tramite.CodigoTramite == (int)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoConFirmaARuego ||
                !SabeFirmar && tramite.CodigoTramite == (int)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoDePersonaInvidente)
                {
                    bool usarTextoBiometriaPlural = parametros.Parametros.Any(p => p.NombreCampo == "TextoBiometriaPlural" && !string.IsNullOrWhiteSpace(p.Valor));
                    var paramsCompa = await ObtenerComparecientesFirmaARuego(datosComparecientes, fotos, grafos, parametricas, usarTextoBiometriaPlural);
                    //parametros.Comparecientes = null;
                    List<Parametro> ParametrosFirmaRuego = parametros.Parametros.ToList();
                    foreach (var p in paramsCompa)
                    {
                        ParametrosFirmaRuego.Add(p);
                    }
                    parametros.Parametros = ParametrosFirmaRuego;
                }
                var timeComparecientes = stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();

                if (!string.IsNullOrWhiteSpace(grafo))
                {
                    var pFirma = parametros.Parametros.Where(p => p.NombreCampo == "FirmaNotario")
                        .FirstOrDefault();
                    if (pFirma != null)
                        pFirma.Valor = grafo;
                }
                if (!string.IsNullOrWhiteSpace(sello))
                {
                    var pSello = parametros.Parametros.Where(p => p.NombreCampo == "SelloNotaria")
                        .FirstOrDefault();
                    if (pSello != null)
                        pSello.Valor = sello;
                }

                parametros.footer = new[] { $"Acta {datosTramite.TipoTramiteCodigo}" };
                documentoPrevio = usarSticker ? _htmlToPdf.CreateSticker(parametros) : _htmlToPdf.CreatePDF(parametros);

                var tCrearPdf = stopwatch.ElapsedMilliseconds;
                stopwatch.Stop();

                parametrosRes.Item2 = JsonConvert.SerializeObject(new { tTramite, tTramiteActa, tNotariaUsuarioActa, tParametrosActa, timeComparecientes, tCrearPdf });
            }
            else
            {
                var notario = await _notarioRepositorio.ObtenerNotarioPrincipal(tramite.NotariaId, new[] { "NotariaUsuarios" });

                if (notario == null) throw new Exception("No se encontró un notario principal");

                var plantilla = await _cache.GetFromCache(string.Format(CACHE_PLANTILLA, tramite.TipoTramiteId), () => _plantillaActaRepositorio.ObtenerPlantillaActaPorTipoTramite(tramite.TipoTramiteId));
                var Sticker = await _cache.GetFromCache(string.Format(CACHE_STICKER, tramite.TipoTramiteId), () => _plantillaActaRepositorio.ObtenerPlantillaStickerPorTipoTramite(tramite.TipoTramiteId));

                string plantilla1 = usarSticker ? Sticker.Contenido : plantilla.Contenido;

                parametrosRes = await ObtenerParametros(tramiteId, tramite.NotariaId, tramite.Fecha, notario.NotariaUsuarios.UserEmail, tramite.DatosAdicionales);

                var parametros = parametrosRes.Item1;
                bool usarTextoBiometriaPlural = parametros.Parametros.Any(p => p.NombreCampo == "TextoBiometriaPlural" && !string.IsNullOrWhiteSpace(p.Valor));
                if (tramite.CodigoTramite == (int)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoConFirmaARuego ||
               !SabeFirmar && tramite.CodigoTramite == (int)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoDePersonaInvidente)
                {
                    var paramsCompa = await ObtenerComparecientesFirmaARuego(datosComparecientes, fotos, grafos, parametricas, usarTextoBiometriaPlural);
                    //parametros.Comparecientes = null;
                    List<Parametro> ParametrosFirmaRuego = parametros.Parametros.ToList();
                    foreach (var p in paramsCompa)
                    {
                        ParametrosFirmaRuego.Add(p);
                    }
                    parametros.Parametros = ParametrosFirmaRuego;
                }

                var prms = parametros.Parametros.ToList();

                if (!string.IsNullOrWhiteSpace(grafo))
                {
                    prms.Add(new Parametro("FirmaNotario", grafo, "Texto"));
                }
                if (!string.IsNullOrWhiteSpace(sello))
                {
                    prms.Add(new Parametro("SelloNotaria", sello, "Texto"));
                }

                parametros.Parametros = prms;
                parametros.Plantilla = plantilla1;

                documentoPrevio = usarSticker ? _htmlToPdf.CreateSticker(parametros) : _htmlToPdf.CreatePDF(parametros);
            }

            return (documentoPrevio, parametrosRes.Item2);
        }

        private async Task<(PlantillaParametros, string)> ObtenerParametros(long tramiteid, long notariaId, DateTime fecha, string notarioUser, string datosAdicionales, string grafo = "", string sello = "")
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            PlantillaParametros paramAux = new PlantillaParametros();

            //var infoPrincipal = await _actaNotarialRepositorio.ObtenerInformacionPrincipalActaNotarial(tramiteid, notarioUser);
            var infoPrincipal = await _cache.GetFromCache(string.Format(CACHE_INFO_ACTA_NOTARIAL, tramiteid, notarioUser), () => _actaNotarialRepositorio.ObtenerInformacionPrincipalActaNotarial(tramiteid, notarioUser));
            var timeTramite = stopWatch.ElapsedMilliseconds;

            stopWatch.Restart();
            //var comparecientes = await _actaNotarialRepositorio.ObtenerComparecientes(tramiteid);
            var comparecientes = await _cache.GetFromCache(string.Format(CACHE_COMPARECIENTES, tramiteid), () => _actaNotarialRepositorio.ObtenerComparecientes(tramiteid));
            var timeComparecientes = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();

            var textoSinBiometria = await _cache.GetFromCache(CACHE_TEXTOSINBIOMETRIA, () => _actaNotarialRepositorio.ObtenerParametrica("TEXTOSINBIOMETRIA"));
            var textoConBiometria = await _cache.GetFromCache(CACHE_TEXTOCONBIOMETRIA, () => _actaNotarialRepositorio.ObtenerParametrica("TEXTOCONBIOMETRIA"));
            var urlQr = await _cache.GetFromCache(CACHE_URLQR, () => _actaNotarialRepositorio.ObtenerParametrica("URLQR"));

            var timeParametricas = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();

            var notaria = await _cache.GetFromCache(string.Format(CACHE_NOTARIA_UBICACION, notariaId),
                () => _notariaRepositorio.GetOneAsync(m => m.NotariaId == notariaId,
                n => n.Ubicacion.UbicacionPadre));
            var timeNotaria = stopWatch.ElapsedMilliseconds;
            stopWatch.Stop();

            List<Parametro> nuevosParametros = new List<Parametro>();
            nuevosParametros.Add(new Parametro("Tramiteid", infoPrincipal.TramiteId.ToString(), "Texto"));
            nuevosParametros.Add(new Parametro("TipoTramite", infoPrincipal.TipoTramite, "Texto"));
            nuevosParametros.Add(new Parametro("Municipio", notaria.Ubicacion?.Nombre, "Texto"));
            nuevosParametros.Add(new Parametro("Departamento", notaria.Ubicacion?.UbicacionPadre?.Nombre, "Texto"));
            nuevosParametros.Add(new Parametro("FechaTramite", infoPrincipal.FechaTramite, "Texto"));
            nuevosParametros.Add(new Parametro("NumeroNotaria", notaria.NumeroNotaria == 0 ? "" : $"({notaria.NumeroNotaria})", "Texto"));
            nuevosParametros.Add(new Parametro("NumeroNotariaEnLetra", notaria.NumeroNotariaEnLetras, "Texto"));
            nuevosParametros.Add(new Parametro("NumeroNotarioEnLetra", notaria.NumeroNotariaEnLetras, "Texto"));
            nuevosParametros.Add(new Parametro("CirculoNotaria", notaria.CirculoNotaria, "Texto"));
            nuevosParametros.Add(new Parametro("NombreNotario", infoPrincipal.NombreNotario, "Texto"));
            nuevosParametros.Add(new Parametro("UrlQR", urlQr?.Valor ?? "", "Texto"));
            nuevosParametros.Add(new Parametro("TipoNotario", infoPrincipal.TipoNotario, "Texto"));
            nuevosParametros.Add(new Parametro("NUT", infoPrincipal.NUT.ToString(), "Texto"));
            nuevosParametros.Add(new Parametro("iddoc", infoPrincipal.iddoc.ToString(), "Texto"));
            nuevosParametros.Add(new Parametro("GeneroNotario", infoPrincipal.GeneroNotario, "Texto"));

            if (comparecientes.Any())
            {
                paramAux.Comparecientes = new List<IEnumerable<Parametro>>();

                foreach (var item in comparecientes)
                {
                    List<Parametro> parametroCompareciente = new List<Parametro>();

                    parametroCompareciente.Add(new Parametro("ComparecienteId", item.ComparecienteId.ToString(), "Texto"));
                    parametroCompareciente.Add(new Parametro("NombreTipoDocumento", item.NombreTipoDocumento, "Texto"));
                    parametroCompareciente.Add(new Parametro("NombreCompareciente", item.NombreCompareciente, "Texto"));
                    parametroCompareciente.Add(new Parametro("NUIPCompareciente", item.NUIPCompareciente, "Texto"));
                    parametroCompareciente.Add(new Parametro("FotoCompareciente", item.FotoCompareciente, "Texto"));
                    parametroCompareciente.Add(new Parametro("FirmaCompareciente", item.FirmaCompareciente, "Texto"));
                    parametroCompareciente.Add(new Parametro("NUT", item.NUT, "Texto"));
                    parametroCompareciente.Add(new Parametro("FechaCompletaNumeros", fecha.ToString("dd/MM/yyyy"), "Texto"));
                    parametroCompareciente.Add(new Parametro("HoraCompletaNumeros", fecha.ToString("hh:mm:ss"), "Texto"));
                    parametroCompareciente.Add(new Parametro("TextoBiometria", item.TramiteSinBiometria ? string.Concat(textoSinBiometria.Valor, " ", item.MotivoSinBiometria) : textoConBiometria.Valor, "Texto"));

                    paramAux.Comparecientes.Add(parametroCompareciente);
                }
            }

            if (!string.IsNullOrWhiteSpace(grafo))
            {
                nuevosParametros.Add(new Parametro("FirmaNotario", grafo, "Texto"));
            }
            if (!string.IsNullOrWhiteSpace(sello))
            {
                nuevosParametros.Add(new Parametro("SelloNotaria", sello, "Texto"));
            }

            var varNUT = nuevosParametros.Where(p => p.NombreCampo == "NUT").FirstOrDefault();
            //var hashids = new Hashids("NUTNOTARIA", 12, "abcdefghijklmnopqrstuvwxyz0123456789");
            //varNUT.Valor = hashids.EncodeLong(Convert.ToInt64(varNUT.Valor));

            //var varIdDoc = nuevosParametros.Where(p => p.NombreCampo == "iddoc").FirstOrDefault();
            //varIdDoc.Valor = hashids.EncodeLong(Convert.ToInt64(varIdDoc.Valor));

            var urlQR = nuevosParametros.Where(p => p.NombreCampo == "UrlQR").FirstOrDefault();
            nuevosParametros.Add(new Parametro("TramiteQR", _generadorQR.StringToQR(urlQR.Valor + varNUT.Valor), "Texto"));
            paramAux.Parametros = nuevosParametros;


            var datosAdicionalesObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(datosAdicionales);

            if (datosAdicionalesObj != null)
            {
                foreach (var kv in datosAdicionalesObj) nuevosParametros.Add(new Parametro() { NombreCampo = kv.Key, Valor = kv.Value });
            }

            return (paramAux, JsonConvert.SerializeObject(new { timeTramite, timeComparecientes, timeNotaria, timeParametricas }));
        }

        private async Task<(DatosTramiteActa, List<DatosComparecienteActa>, List<DatosImagenActa>, List<DatosImagenActa>)>
           ObtenerTramiteActa(long tramiteId, bool actaSticker, bool IncluirInfoComparecientes = true)
        {
            (DatosTramiteActa, List<DatosComparecienteActa>, List<DatosImagenActa>, List<DatosImagenActa>) datos;

            var tramiteQ = _tramiteRepositorio.ObtenerTodo()
            .Where(t => !t.IsDeleted &&
                    t.TramiteId == tramiteId);

            var datosTramite = await tramiteQ.Select(t => new DatosTramiteActa()
            {
                TramiteId = t.TramiteId,
                TipoTramite = t.TipoTramite.Nombre,
                TipoTramiteCodigo = t.TipoTramite.CodigoTramite,
                FechaTramite = t.Fecha,
                DatosAdicionales = t.DatosAdicionales,
                UsarSticker = actaSticker,
                Plantilla = actaSticker ? t.TipoTramite.PlantillaSticker.Contenido : t.TipoTramite.PlantillaActa.Contenido,
                Plantilla2 = actaSticker ? t.TipoTramite.PlantillaDosSticker.Contenido : null,
                DireccionComparecencia = t.DireccionComparecencia,
                FueraDeDespacho = t.FueraDeDespacho
            }).FirstOrDefaultAsync();

            if (IncluirInfoComparecientes)
            {
                var datosComparecientes = await _comparecienteRepositorio.ObtenerTodo()
                    .Where(c => c.TramiteId == tramiteId)
                    .Select(c => new DatosComparecienteActa()
                    {
                        NombreTipoDocumento = c.Persona.TipoIdentificacion.Nombre,
                        Nombres = c.Persona.Nombres,
                        Apellidos = c.Persona.Apellidos,
                        NUIPCompareciente = c.Persona.NumeroDocumento,
                        NUT = c.PeticionRNEC,
                        FechaCreacion = c.FechaCreacion,
                        TramiteSinBiometria = c.TramiteSinBiometria,
                        MotivoSinBiometria = c.MotivoSinBiometria,
                        FirmaId = c.FirmaId,
                        FotoId = c.FotoId,
                        Posicion = c.Posicion == null ? 1 : Convert.ToInt32(c.Posicion)
                    }).ToListAsync();


                var fotosComparecientes = await _actaNotarialRepositorio.ObtenerTodo()
                .Where(c => datosComparecientes.Select(c => c.FotoId).Contains(c.MetadataArchivoId))
                .Select(a => new DatosImagenActa()
                {
                    MetadataArchivoId = a.MetadataArchivoId,
                    Contenido = a.Archivo.Contenido
                }).ToListAsync();

                var grafosComparecientes = await _actaNotarialRepositorio.ObtenerTodo()
                    .Where(c => datosComparecientes.Select(c => c.FirmaId).Contains(c.MetadataArchivoId))
                    .Select(a => new DatosImagenActa()
                    {
                        MetadataArchivoId = a.MetadataArchivoId,
                        Contenido = a.Archivo.Contenido
                    }).ToListAsync();


                datos = (datosTramite, datosComparecientes, fotosComparecientes, grafosComparecientes);
            }
            else
            {
                datos = (datosTramite, null, null, null);
            }
            return datos;
        }
    }
}