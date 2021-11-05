using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using GenericExtensions;
using Infraestructura.Transversal.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class PlantillaActaServicio : BaseServicio, IPlantillaActaServicio
    {
        private IPlantillaActaRepositorio _plantillaActaRepositorio { get; }
        private ITipoTramiteRepositorio _tipoTramiteRepositorio { get; }

        public PlantillaActaServicio(IPlantillaActaRepositorio plantillaActaRepositorio
            ,ITipoTramiteRepositorio tipoTramiteRepositorio):base(plantillaActaRepositorio,tipoTramiteRepositorio)
        {
            _plantillaActaRepositorio = plantillaActaRepositorio;
            _tipoTramiteRepositorio = tipoTramiteRepositorio;
        }

        public async Task Crear(PlantillaActaCreateDTO plantillaActaCreateDTO)
        {
            var plantillaActa = plantillaActaCreateDTO.Adaptar<PlantillaActa>();
             _plantillaActaRepositorio.Agregar(plantillaActa);
            _plantillaActaRepositorio.UnidadDeTrabajo.Commit();
        }

        public async Task Actualizar(PlantillaActaEditDTO plantillaActaEditDTO)
        {
            var plantilla = _plantillaActaRepositorio.Obtener(plantillaActaEditDTO.PlantillaActaId);
            if (plantilla == null)
                throw new NotFoundException("Plantilla no encontrada");
            plantilla.Contenido = plantillaActaEditDTO.Contenido;
            plantilla.Nombre = plantillaActaEditDTO.Nombre;
            plantilla.FechaModificacion =DateTime.Now;
            _plantillaActaRepositorio.Modificar(plantilla);
            _plantillaActaRepositorio.UnidadDeTrabajo.Commit();
        }

        public async Task AsociarTiposTramites(PlantillaActaAssociateDTO plantillaActaAssociateDTO)
        {
            List<TipoTramite> tiposTramitesActualizar = new List<TipoTramite>();
            foreach(long codigo in plantillaActaAssociateDTO.CodigosTramites)
            {
                var tipotramite = (await _tipoTramiteRepositorio.Obtener(t=> t.CodigoTramite==codigo)).FirstOrDefault();
                if (tipotramite != null)
                {
                    tipotramite.PlantillaActaId = plantillaActaAssociateDTO.PlantillaActaId;
                    tiposTramitesActualizar.Add(tipotramite);
                }
            }
            if (tiposTramitesActualizar.Count() > 0)
            {
                _tipoTramiteRepositorio.Modificar(tiposTramitesActualizar);
                _tipoTramiteRepositorio.UnidadDeTrabajo.Commit();
            }
            else
                throw new NotFoundException("No se encontraron trámites para actualizar");
            
        }

        public async Task<IEnumerable<PlantillaActaReturnDTO>> ObtenerPlantillas()
        {
            var plantillas = (await _plantillaActaRepositorio
                                .Obtener(p=>p.IsDeleted==false,
                                p=>p.TiposTramites)).ToList();
            if (plantillas == null)
                throw new NotFoundException("No se encontraron actas");
            return plantillas.Select(p => p.Adaptar<PlantillaActaReturnDTO>());
        }

        public async Task<PlantillaActaFullReturnDTO> ObtenerPlantillaPorId(long Id)
        {
            var plantilla = (await _plantillaActaRepositorio
                                .Obtener(p => p.IsDeleted == false && p.PlantillaActaId==Id, 
                                p=>p.TiposTramites))
                                .FirstOrDefault();
            if (plantilla == null)
                throw new NotFoundException("Plantilla no encontrada");
            return plantilla.Adaptar<PlantillaActaFullReturnDTO>();
        }
    }
}
