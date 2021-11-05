using ApiGateway.Models.Transaccional;
using Newtonsoft.Json;
using PortalCliente.Data.DatosTramite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortalCliente.Data
{
    public class Tramite
    {
        public long TramiteId { get; set; }
        public TipoTramite TipoTramite { get; set; }
        public int CantidadComparecientes { get; set; }
        public int ComparecienteActualPos { get; set; }
        public bool ComparecientesCompletos { get; set; }
        public bool UsarSticker { get; set; }
        public bool FueraDeDespacho { get; set; }
        public string DireccionComparecencia { get; set; }
        public List<Compareciente> Comparecientes { get; set; }
        private Compareciente _comparecienteActual;
        public ActaCreate InfoActa { get; set; }

        public Compareciente ComparecienteActual
        {
            get => _comparecienteActual;
            set
            {
                _comparecienteActual = value;
                _comparecienteActual.Tramite = this;
            }
        }
        public string DatosAdicionales { get; internal set; }
        public static Tramite ObtenerNuevoTramite()
        {
            var ret = new Tramite
            {
                CantidadComparecientes = 1,
                ComparecienteActualPos = 1,
                ComparecientesCompletos = false,
                Comparecientes = new List<Compareciente>(),
                InfoActa = new ActaCreate()
            };
            return ret;
        }

        public string IsValid(long CodigoTramite)
        {
            if (FueraDeDespacho && string.IsNullOrWhiteSpace(DireccionComparecencia))
                return "Ingrese la dirección de comparecencia";

            string respuesta = string.Empty;
            switch (CodigoTramite)
            {
                case (int)TipoTramiteId.AutenticacionDeHuella:
                    if (!string.IsNullOrEmpty(DatosAdicionales))
                    {
                        var datos = JsonConvert.DeserializeObject<EnrolamientoNotariaDigitalDTO>(DatosAdicionales);
                        var telRegex = new Regex(@"^3\d{9}$");
                        if (string.IsNullOrWhiteSpace(datos.Telefono) ||
                            !telRegex.IsMatch(datos.Telefono) )
                            respuesta = "Ingrese el teléfono para continuar*";

                        var mailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                        if (string.IsNullOrWhiteSpace(datos.Correo.Trim()) ||
                            !mailRegex.IsMatch(datos.Correo))
                            respuesta = "Ingrese correo para continuar*";
                    }
                    else
                        respuesta = "Ingrese el teléfono y correo el  para continuar";
                    break;
                case (int)TipoTramiteId.Autenticacion_extraproceso:
                    if (!string.IsNullOrEmpty(DatosAdicionales))
                    {
                        var datos = JsonConvert.DeserializeObject<DeclaracionExtraProcesoDTO>(DatosAdicionales);
                        if (string.IsNullOrEmpty(datos.ActoNotarial.Trim()))
                            respuesta = "Ingrese el título del trámite para continuar*";
                    }
                    else
                        respuesta = "Ingrese el título del trámite para continuar";
                    break;
                case (int)TipoTramiteId.Diligencia_Personal:
                    if (!string.IsNullOrEmpty(DatosAdicionales))
                    {
                        var datos = JsonConvert.DeserializeObject<PresentacionPersonalDTO>(DatosAdicionales);
                        if (string.IsNullOrEmpty(datos.Destinatario.Trim()))
                            respuesta = "Ingrese el destinatario para continuar*";
                    }
                    else
                        respuesta = "Ingrese el destinatario para continuar";
                    break;
                case (int)TipoTramiteId.Diligencia_registro_civil:
                    if (!string.IsNullOrEmpty(DatosAdicionales))
                    {
                        var datos = JsonConvert.DeserializeObject<InscripcionRegistroCivilDTO>(DatosAdicionales);
                        if (string.IsNullOrEmpty(datos?.TipoRegistroCivil?.Trim()) || string.IsNullOrEmpty(datos?.NombresApellidosInscritos?.Trim()))
                            respuesta = "Complete los datos requeridos para el registro civil*";
                    }
                    else
                        respuesta = "Complete los datos requeridos para el registro civil";
                    break;
                case (int)TipoTramiteId.AutenticacionBiometrica_escriturapublica:
                    if (!string.IsNullOrEmpty(DatosAdicionales))
                    {
                        var datos = JsonConvert.DeserializeObject<EscrituraPublicaDTO>(DatosAdicionales);
                        if (datos.FechaTramite > DateTime.Today)
                            respuesta = "No esta permitido seleccionar fechas futuras";
                    }
                    else
                        respuesta = string.Empty;
                    break;

                default:
                    break;
            }
            return respuesta;
        }
    }

    public class ConsultaTramite
    {
        public long TramiteId { get; set; }
        public long TipoTramiteId { get; set; }
        public bool EsNuevo { get; set; }
    }

    public enum CodigoTipoTramite
    {
        DocumentoPrivado = 1,
        EscrituraPublica = 2,
        DeclaracionExtraProceso = 3,
        PresentacionPersonal = 4,
        InscripcionRegistroCivil = 6,
        DocumentoPrivadoFirmaARuego = 7,
        DocumentoPrivadoInvidente = 8,
        AutenticacionFirma = 9,
        EnrolamientoNotariaDigital = 10,
    }

    public enum TipoTramiteId
    {
        AutenticacionDeHuella = 1,
        AutenticacionBiometrica_escriturapublica = 7,
        Autenticacion_extraproceso = 8,
        Diligencia_Personal = 9,
        Diligencia_registro_civil = 10
    }
}
