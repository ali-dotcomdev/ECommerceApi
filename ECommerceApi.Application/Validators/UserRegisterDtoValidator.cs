using ECommerceApi.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanici adi bos birakilamaz")
            .MinimumLength(3).WithMessage("Kullanici adi minimum 3 karakterli olmalidir")
            .MaximumLength(50).WithMessage("Kullanici adi maksimum 50 karakter olabilir");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresi bos birakilamaz")
            .EmailAddress().WithMessage("lutfen gecerli bir email adresi giriniz");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Sifre kismi bos birakilamaz")
            .MinimumLength(6).WithMessage("Sifreniz 6 karakterden uzun olmali");

        //RuleFor(x => x.ConfirmPassword)
        //    .Equal(x => x.Password).WithMessage("Sifreler uyusmuyor");
    }
}
