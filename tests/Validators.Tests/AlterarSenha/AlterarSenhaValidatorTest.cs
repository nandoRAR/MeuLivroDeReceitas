using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaOsTestes.Requests;
using Xunit;

namespace Validators.Tests.AlterarSenha;

public class AlterarSenhaValidatorTest
{
    [Fact]
    public void Sucesso()
    {
        var validator = new AlterarSenhaValidator();
        var request = RequestAlterarSenhaUsuarioBuilder.Construir();
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Vidalidar_Erro_Senha_Invalida(int tamanhoSenha)
    {
        var validator = new AlterarSenhaValidator();

        var request = RequestAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
    }

    [Fact]
    public void Vidalidar_Erro_Senha_Vazio()
    {
        var validator = new AlterarSenhaValidator();

        var request = RequestAlterarSenhaUsuarioBuilder.Construir();
        request.NovaSenha = string.Empty;
        var resultado = validator.Validate(request);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(
            ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }
}
