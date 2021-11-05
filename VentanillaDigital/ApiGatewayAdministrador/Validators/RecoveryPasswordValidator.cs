using ApiGateway.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Validators
{
    public class RecoveryPasswordValidator : AbstractValidator<RecoveryPasswordRequest>
    {
        public RecoveryPasswordValidator()
        {
            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
