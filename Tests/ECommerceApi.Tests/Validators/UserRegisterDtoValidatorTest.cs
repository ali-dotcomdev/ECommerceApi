using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Tests.Validators;

public class UserRegisterDtoValidatorTest
{
    private readonly UserRegisterDtoValidator _validator;
    public UserRegisterDtoValidatorTest()
    {
        _validator = new UserRegisterDtoValidator();
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenAllFieldsAreValid()
    {
        var model = new UserRegisterDto
        {
            Username = "username",
            Email = "user@gmail.com",
            Password = "password123"
        };

        var result = _validator.Validate(model);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("aa")]
    [InlineData(null)]
    public void Validate_ShouldReturnFalse_WhenUsernameIsInvalid(string invalidUsername)
    {
        var model = new UserRegisterDto
        {
            Username = invalidUsername,
            Email = "user@gmail.com",
            Password = "password123"
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, u => u.PropertyName == "Username");
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenEmailIsEmpty()
    {
        var model = new UserRegisterDto
        {
            Username = "username",
            Email = "",
            Password = "password123"
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("")]
    public void Validate_ShouldReturnFalse_WhenPasswordIsInvalid(string invalidPassword)
    {
        var model = new UserRegisterDto
        {
            Username = "username",
            Email = "user@gmail.com",
            Password = invalidPassword
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, p => p.PropertyName == "Password");
    }
}
