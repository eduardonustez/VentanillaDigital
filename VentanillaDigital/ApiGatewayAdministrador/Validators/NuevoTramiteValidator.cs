using ApiGateway.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Validators
{
    public class NuevoTramiteValidator : AbstractValidator<TramiteModel>
    {
        public NuevoTramiteValidator()
        {
            RuleFor(p => p.CantidadComparecientes)
            .GreaterThanOrEqualTo(1).NotNull().NotEmpty();

            RuleFor(p => p.TipoTramiteId).NotEmpty().NotNull();
        }
    }
}
