using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Infraestructura.Transversal.Template;
using Infraestructura.Transversal.Template.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class TemplateServicio : BaseServicio, ITemplateServicio
    {
        #region MyRegion
        private const string CODIGOPLANTILLARECUPERAR = "TEMPLATERECOVERY";
        private const string CODIGOPLANTILLANOTIFICACIONTRAMITECLIENTE = "TEMPLATENOTIFICACIONTRAMITECLIENTE";
        private const string CODIGOPLANTILLANOTIFICACIONUSUARIOADMIN = "TEMPLATENOTIFICACIONUSUARIOADMIN";
        #endregion
        public ITemplateRepositorio _templateRepositorio { get; }
        public TemplateServicio(ITemplateRepositorio templateRepositorio) : base(templateRepositorio)
        {
            _templateRepositorio = templateRepositorio;
        }

        public string ObtenerTemplateRecuperacionPassword(string email, string enlace)
        {
            var informacionUsuario = _templateRepositorio.ObtenerInformacionUsuario(email);
            var template = _templateRepositorio.ObtenerTemplate(CODIGOPLANTILLARECUPERAR);

            ViewModelRecuperarPassword viewModelRecuperar = new ViewModelRecuperarPassword
            {
                NombreNotario = informacionUsuario.Usuario,
                DescripcionNotaria = informacionUsuario.NombreNotaria,
                CallbackUrl = enlace
            };
            TemplateBodyHTML.ObtenerCuerpoHTML(template, viewModelRecuperar, out string value);
            return value;
        }

        public string ObtenerTemplateNotificacionTramiteCliente(string enlace, string nombreCliente)
        {
            //var informacionUsuario = _templateRepositorio.ObtenerInformacionUsuario(email);
            var template = _templateRepositorio.ObtenerTemplate(CODIGOPLANTILLANOTIFICACIONTRAMITECLIENTE);

            var viewModelRecuperar = new ViewModelEnvioNotificacionTramiteCliente
            {
                NombreCliente = nombreCliente,
                //NombreNotario = informacionUsuario.Usuario,
                //DescripcionNotaria = informacionUsuario.NombreNotaria,
                CallbackUrl = enlace
            };

            TemplateBodyHTML.ObtenerCuerpoHTML(template, viewModelRecuperar, out string value);
            return value;
        }

        public string ObtenerTemplateAsignacionClave(string email, string enlace)
        {
            var template = _templateRepositorio.ObtenerTemplate(CODIGOPLANTILLANOTIFICACIONUSUARIOADMIN);

            var viewModel = new ViewModelEnvioNotificacionTramiteCliente
            {
                CallbackUrl = enlace
            };

            TemplateBodyHTML.ObtenerCuerpoHTML(template, viewModel, out string value);
            return value;
        }
    }
}
