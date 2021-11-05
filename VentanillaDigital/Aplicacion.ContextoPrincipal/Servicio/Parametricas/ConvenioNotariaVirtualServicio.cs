using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Microsoft.Extensions.Options;
using GenericExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio.Parametricas
{
    public class ConvenioNotariaVirtualServicio : BaseServicio, IConvenioNotariaVirtualServicio
    {
        private readonly MiFirmaSettings _miFirmaSettings;

        private IConvenioNotariaVirtualRepositorio _convenioNotariaVirtualRepositorio { get; }
        private IEstadoTramiteVirtualRepositorio _estadoTramiteVirtualRepositorio { get; }
        private IParametricaRepositorio _parametricaRepositorio { get; }
        public ConvenioNotariaVirtualServicio(
            IConvenioNotariaVirtualRepositorio convenioNotariaVirtualRepositorio
            , IEstadoTramiteVirtualRepositorio estadoTramiteVirtualRepositorio
            , IParametricaRepositorio parametricaRepositorio
            , IOptions<MiFirmaSettings> miFirmaSettings)
            : base(convenioNotariaVirtualRepositorio
                  , estadoTramiteVirtualRepositorio
                  , parametricaRepositorio)
        {
            _convenioNotariaVirtualRepositorio = convenioNotariaVirtualRepositorio;
            _estadoTramiteVirtualRepositorio = estadoTramiteVirtualRepositorio;
            _parametricaRepositorio = parametricaRepositorio;
            _miFirmaSettings = miFirmaSettings.Value;
        }
        public async Task<bool> ObtenerNotariaVirtualConvenio(ConvenioNotariaVirtualDTO convenioNotaria)
        {
            try
            {
                bool esNotariaConConvenio = (await _convenioNotariaVirtualRepositorio.Obtener(x => x.NotariaId == convenioNotaria.NotariaId)).Any();
                return esNotariaConConvenio;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<EstadosTramitesVirtualResponseDTO>> ObtenerEstadosTramiteVirtual(EstadosTramiteVirtualDTO estadosTramite)
        {
            List<EstadosTramitesVirtualResponseDTO> responseDTO = new List<EstadosTramitesVirtualResponseDTO>();
            var estados = await _estadoTramiteVirtualRepositorio.Obtener(x => x.IsDeleted == estadosTramite.IsDeleted);
            if (estados != null)
            {
                foreach (var item in estados.ToList())
                {
                    responseDTO.Add(new EstadosTramitesVirtualResponseDTO
                    {
                        EstadoTramiteVirtualId = item.EstadoTramiteID,
                        Descripcion = item.Nombre
                    });
                }
            }
            return responseDTO;
        }

        public async Task<ConfiguracionMiFirmaDTO> ObtenerMiConfiguracionMiFirma(ConvenioNotariaVirtualDTO convenioNotaria)
        {
            ConfiguracionMiFirmaDTO esConfiguracionMiFirma = (await _convenioNotariaVirtualRepositorio.Obtener(x => x.NotariaId == convenioNotaria.NotariaId)).Select(x => new ConfiguracionMiFirmaDTO()
            {
                MyFrame = _parametricaRepositorio.Obtener(x => x.Codigo == _miFirmaSettings.BuscarPorFrameMiFirma).Result.FirstOrDefault()?.Valor,

                ChannelAuthMiFirma = _parametricaRepositorio.Obtener(x => x.Codigo == _miFirmaSettings.BuscarPorChannelAuth).Result.FirstOrDefault()?.Valor,

                Gateway = _parametricaRepositorio.Obtener(x => x.Codigo == _miFirmaSettings.BuscarPorGateway).Result.FirstOrDefault()?.Valor,

                ConfigurationGuid = x.ConfigurationGuid,

                LoginConvenio = x.LoginConvenio,

                PasswordConvenio = x.PasswordConvenio
            }).FirstOrDefault();

            return esConfiguracionMiFirma;
        }
    }
}
