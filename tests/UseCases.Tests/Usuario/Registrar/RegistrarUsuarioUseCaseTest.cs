using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.ParaOsTestes.Criptografria;
using Utilitario.ParaOsTestes.Mapper;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.Requests;
using Utilitario.ParaOsTestes.Token;
using Xunit;

namespace UseCases.Tests.Usuario.Registrar;

public class RegistrarUsuarioUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        var requisicao = RequestRegistrarUsuarioBuilder.Construir();
        var useCase = CriarUseCase();
        var resposta = await useCase.Executar(requisicao);

        resposta.Should().NotBeNull();
        resposta.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validar_Erro_Email_Ja_Registrado()
    {
        var requisicao = RequestRegistrarUsuarioBuilder.Construir();
        var useCase = CriarUseCase(requisicao.Email);

        Func<Task> acao = async () => { await useCase.Executar(requisicao); };
        
        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exeption => exeption.MensagensDeErro.Count == 1 && 
            exeption.MensagensDeErro.Contains(ResourceMensagensDeErro.EMAIL_JA_CADASTRADO));
    }

    [Fact]
    public async Task Validar_Erro_Email_Vazio()
    {
        var requisicao = RequestRegistrarUsuarioBuilder.Construir();
        requisicao.Email = string.Empty;
        var useCase = CriarUseCase();

        Func<Task> acao = async () => { await useCase.Executar(requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exeption => exeption.MensagensDeErro.Count == 1 &&
            exeption.MensagensDeErro.Contains(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
    }

    private static RegistrarUsuarioUseCase CriarUseCase(string email ="")
    {
        var mapper = MapperBuilder.Instancia();
        var repositorio = UsuarioWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var encriptador = EncriptadorDeSenhaBuilder.Instancia();
        var token = TokenControllerBuilder.Instancia();
        var repositorioReadOnly = UsuarioReadOnlyRepositorioBuilder.Instancia().ExisteUsuarioComEmail(email).Construir();
        return new RegistrarUsuarioUseCase(repositorio, mapper, unidadeDeTrabalho, encriptador, token, repositorioReadOnly);
    }
}
