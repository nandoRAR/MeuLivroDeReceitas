using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Mapper;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.Requests;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Tests.Receita.Registrar;

public class RegistrarReceitaUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequestReceitaBuilder.Construir();

        var resposta = await useCase.Executar(requisicao);

        resposta.Should().NotBeNull();

        resposta.Id.Should().NotBeNullOrWhiteSpace();
        resposta.Titulo.Should().Be(requisicao.Titulo);
        resposta.Categoria.Should().Be(requisicao.Categoria);
        resposta.ModoPreparo.Should().Be(requisicao.ModoPreparo);
    }

    [Fact]
    public async Task Validar_Erro_Ingrediente_Vazio()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequestReceitaBuilder.Construir();
        requisicao.Ingredientes.Clear();

        Func<Task> acao = async () => { await useCase.Executar(requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE));
    }


    private static RegistrarReceitaUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var mapper = MapperBuilder.Instancia();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var repositorio = ReceitaWriteOnlyRepositorioBuilder.Instancia().Construir();
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();

        return new RegistrarReceitaUseCase(mapper, unidadeDeTrabalho, usuarioLogado, repositorio);
    }
}
