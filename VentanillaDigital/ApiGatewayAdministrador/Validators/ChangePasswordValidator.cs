using ApiGatewayAdministrador.Helper;
using ApiGateway.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
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
                .Equal(x => x.ConfirmPassword )
                .WithMessage(ExpresionRegularModelos.PasswordDiferentes)
                .Matches(ExpresionRegularModelos.Password)
                .WithMessage(ExpresionRegularModelos.MensajePassword)
                .NotNull()
                .NotEmpty();
        }
    }
}
