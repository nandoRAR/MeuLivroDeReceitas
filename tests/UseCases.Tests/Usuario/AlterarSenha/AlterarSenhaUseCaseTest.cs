using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaOsTestes.Criptografria;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;
using Utilitario.ParaOsTestes.Requests;

namespace UseCases.Tests.Usuario.AlterarSenha;

public class AlterarSenhaUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();
        var useCase = CriarUseCase(usuario);

        var requisicao = RequestAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };
        await acao.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validar_Erro_NovaSenhaEmBranco()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();
        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequestAlterarSenhaJson
            {
                SenhaAtual = senha,
                NovaSenha = ""
            });
        };
        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(
                ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }

    [Fact]
    public async Task Validar_Erro_SenhaAtual_Invalida()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();
        var useCase = CriarUseCase(usuario);

        var requisicao = RequestAlterarSenhaUsuarioBuilder.Construir();

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };
        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(
                ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validar_Erro_NovaSenha_Invalida(int tamanhoSenha)
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();
        var useCase = CriarUseCase(usuario);

        var requisicao = RequestAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };
        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(
                ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
    }

    private static AlterarSenhaUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var encriptador = EncriptadorDeSenhaBuilder.Instancia();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var repositorio = UsuarioUpdateOnlyRepositorioBuilder.Instancia().RecuperarPorId(usuario).Construir();
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();

        return new AlterarSenhaUseCase(repositorio, usuarioLogado, encriptador, unidadeDeTrabalho);
    }
}
