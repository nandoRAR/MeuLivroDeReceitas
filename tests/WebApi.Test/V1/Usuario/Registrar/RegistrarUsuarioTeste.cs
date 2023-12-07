using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Requests;
using Xunit;

namespace WebApi.Test.V1.Usuario.Registrar;

public class RegistrarUsuarioTeste : ControllerBase
{
    private const string METODO = "usuario";

    public RegistrarUsuarioTeste(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {

    }

    [Fact]
    public async Task Validar_Error_Nome_Vazio()
    {
        var requisicao = RequestRegistrarUsuarioBuilder.Construir();
        requisicao.Nome = "";
        var resposta = await PostRequest(METODO, requisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var respostaBody = await resposta.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        var erros = respostaData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(c => c.GetString().Equals(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO));
    }

}
