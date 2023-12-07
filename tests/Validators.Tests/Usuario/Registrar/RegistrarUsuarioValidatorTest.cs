using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaOsTestes.Requests;
using Xunit;

namespace Validators.Tests.Usuario.Registrar;

public class RegistrarUsuarioValidatorTest
{
    [Fact]
    public void Sucesso()
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir();
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Vidalidar_Erro_Nome_Vazio()
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir();
        request.Nome = string.Empty;
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Vidalidar_Erro_Email_Vazio()
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir();
        request.Email = string.Empty;
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Vidalidar_Erro_Senha_Vazio()
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir();
        request.Senha = string.Empty;
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Vidalidar_Erro_Telefone_Vazio()
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir();
        request.Telefone = string.Empty;
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.TELEFONE_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Vidalidar_Erro_Email_Invalido()
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir();
        request.Email = "we";
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO));
    }

    [Fact]
    public void Vidalidar_Erro_Telefone_Invalido()
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir();
        request.Telefone = "32222";
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.TELEFONE_USUARIO_INVALIDO));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Vidalidar_Erro_Senha_Invalida(int tamanhoSenha)
    {
        var validator = new RegistrarUsuarioValidator();

        var request = RequestRegistrarUsuarioBuilder.Construir(tamanhoSenha);
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
    }
}
