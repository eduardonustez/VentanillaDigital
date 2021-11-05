using ApiGateway.Models.Transaccional;
using Newtonsoft.Json;
using PortalAdministrador.Data.DatosTramite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Data
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
                        if (string.IsNullOrEmpty(datos?.NombresApellidosInscritos?.Trim()))
                            respuesta = "Ingrese los nombres y apellidos del Registro*";
                    }
                    else
                        respuesta = "Ingrese los nombres y apellidos del Registro";
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
        AutenticacionHuella = 5,
        InscripcionRegistroCivil = 6,
        DocumentoPrivadoFirmaARuego = 7,
        DocumentoPrivadoInvidente = 8,
        AutenticacionFirma = 9
    }

    public enum TipoTramiteId
    {
        Autenticacion_extraproceso = 8,
        Diligencia_Personal = 9,
        Diligencia_registro_civil = 10
    }
}
