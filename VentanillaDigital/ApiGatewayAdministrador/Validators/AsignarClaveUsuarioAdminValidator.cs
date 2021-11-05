using ApiGateway.Contratos.Models.Account;
using ApiGatewayAdministrador.Helper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Validators
{
    public class AsignarClaveUsuarioAdminValidator : AbstractValidator<AsignarClaveUsuarioAdminRequest>
    {
        public AsignarClaveUsuarioAdminValidator()
        {
            RuleFor(p => p.Code)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(p => p.Password)
                .Equal(x => x.ConfirmPassword)
                .WithMessage(ExpresionRegularModelos.PasswordDiferentes)
                .Matches(ExpresionRegularModelos.Password)
                .WithMessage(ExpresionRegularModelos.MensajePassword)
                .NotNull()
                .NotEmpty();
        }
    }
}
