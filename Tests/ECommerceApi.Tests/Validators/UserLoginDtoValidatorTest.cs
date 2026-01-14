using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ECommerceApi.Tests.Validators;

public class UserLoginDtoValidatorTest
{
    private readonly UserLoginDtoValidator _validator;

    public UserLoginDtoValidatorTest()
    {
        _validator = new UserLoginDtoValidator();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenEmailIsEmpty()
    {
        // Arrange
        var model = new UserLoginDto
        {
            Email = "",
            Password = "password123"
        };

        // Act 
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);

        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        var model = new UserLoginDto
        {
            Email = "user@gmail.com",
            Password = "password123"
        };

        var result = _validator.Validate(model);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345")]
    [InlineData("")]
    public void Validate_ShouldReturnError_WhenPasswordIsTooShort(string incorrectPassword)
    {
        var model = new UserLoginDto
        {
            Email = "user@gmail.com",
            Password = incorrectPassword
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, p => p.PropertyName == "Password");
    }
}
