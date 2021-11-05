﻿using ApiGateway.Models;
using FluentValidation;

namespace ApiGateway.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(p => p.NumeroIdentificacion)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(6, 12);
        }
    }
}
