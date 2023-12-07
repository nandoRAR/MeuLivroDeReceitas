using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Requests;
using Xunit;

namespace WebApi.Test.V1.Login.FazerLogin;

public class LoginTest : ControllerBase
{
    private const string METODO = "login";
    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public LoginTest(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.RecuperarUsuario();
        _senha = factory.RecuperarSenha();
    }

    [Fact]
    public async Task Validar_Erro_Email_Invalido()
    {
        var requisicao = new MeuLivroDeReceitas.Comunicacao.Request.RequestLoginJson
        {
            Email = "email@invalido.com",
            Senha = _senha
        };
        var resposta = await PostRequest(METODO, requisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var respostaBody = await resposta.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        var erros = respostaData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
        erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.LOGIN_INVALIDO);
    }
}
