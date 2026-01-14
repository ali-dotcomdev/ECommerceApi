using ECommerceApi.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Validators;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresi bos birakilamaz")
            .EmailAddress().WithMessage("Gecerli bir email adresi giriniz");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Sifre bos birakilamaz")
            .MinimumLength(6).WithMessage("Sifre en az 6 karakterli olmalidir.");
    }
}
