using System.ComponentModel.DataAnnotations;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.Tests;

public sealed class InputValidationTests
{
    private static IList<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }

    private static RegisterRequest Register(string first = "Maria", string last = "Perez", string password = "Prueba123", string email = "maria@test.com") =>
        new() { Email = email, Password = password, FirstName = first, LastName = last };

    [Fact]
    public void Valid_registration_with_accents_hyphen_and_apostrophe_passes()
    {
        Assert.Empty(Validate(Register("María José", "O'Brien-Núñez")));
    }

    [Theory]
    [InlineData("Juan1")]
    [InlineData("123")]
    [InlineData("Ana@")]
    [InlineData("J")]
    [InlineData("  ")]
    [InlineData("<script>")]
    public void Names_with_digits_symbols_or_too_short_are_rejected(string name)
    {
        Assert.NotEmpty(Validate(Register(first: name)));
        Assert.NotEmpty(Validate(Register(last: name)));
    }

    [Fact]
    public void Name_longer_than_fifty_characters_is_rejected() =>
        Assert.NotEmpty(Validate(Register(first: new string('a', 51))));

    [Theory]
    [InlineData("sololetras1", true)]
    [InlineData("soloLetras", false)]
    [InlineData("12345678", false)]
    [InlineData("corta1", false)]
    public void Password_needs_letter_and_digit_and_length(string password, bool valid) =>
        Assert.Equal(valid, !Validate(Register(password: password)).Any());

    [Fact]
    public void Password_and_email_have_maximum_length()
    {
        Assert.NotEmpty(Validate(Register(password: "a1" + new string('x', 70))));
        Assert.NotEmpty(Validate(Register(email: new string('a', 250) + "@x.com")));
    }

    [Theory]
    [InlineData("70123456", true)]
    [InlineData("7012 3456", false)]
    [InlineData("abcdefg", false)]
    [InlineData("123", false)]
    [InlineData("1234567890123456", false)]
    public void Phone_accepts_only_digits_within_range(string phone, bool valid) =>
        Assert.Equal(valid, !Validate(new UpdateStudentProfileRequest { Phone = phone }).Any());

    [Theory]
    [InlineData("Cochabamba", true)]
    [InlineData("Sta. Cruz", true)]
    [InlineData("Colegio 6", false)]
    [InlineData("Tarija#", false)]
    public void City_accepts_only_letters(string city, bool valid) =>
        Assert.Equal(valid, !Validate(new UpdateStudentProfileRequest { City = city }).Any());

    [Theory]
    [InlineData(1999, false)]
    [InlineData(2026, true)]
    [InlineData(2051, false)]
    public void Graduation_year_has_a_sane_range(int year, bool valid) =>
        Assert.Equal(valid, !Validate(new UpdateStudentProfileRequest { GraduationYear = year }).Any());

    [Theory]
    [InlineData("alguien@gmail.com", true)]
    [InlineData("alguien+2@gmail.com", false)]
    [InlineData("x@mailinator.com", false)]
    [InlineData("x@YOPMAIL.com", false)]
    [InlineData("sin-arroba", false)]
    [InlineData("x@dominio", false)]
    public void Registration_policy_blocks_aliases_and_disposable_domains(string email, bool accepted) =>
        Assert.Equal(accepted, RegistrationPolicy.CheckEmail(email) is null);

    [Fact]
    public void Catalog_numbers_have_limits()
    {
        Assert.NotEmpty(Validate(new CompatibilityRequest { CareerId = Guid.NewGuid(), Dimension = "C", CompatibilityLevel = 101, Rule = "x" }));
        Assert.NotEmpty(Validate(new CompatibilityRequest { CareerId = Guid.NewGuid(), Dimension = "Z", CompatibilityLevel = 50, Rule = "x" }));
        Assert.NotEmpty(Validate(new CareerRequest { Name = "X", Status = "OK", Cost = 999999999 }));
        Assert.NotEmpty(Validate(new CareerRequest { Name = "X", Status = "OK", DurationMonths = 100000 }));
        Assert.Empty(Validate(new CareerRequest { Name = "X", Status = "OK", Cost = 500, DurationMonths = 60 }));
    }
}
