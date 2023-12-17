using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarUsuario;
using MeuLivroDeReceitas.Application.UseCases.Usuario.RecuperarPerfil;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Comunicacao.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;

public class UsuarioController : MeuLivroDeReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(ReponseUsuarioRegistradoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarUsuario([FromServices] IRegistrarUsuarioUseCase useCase,
        [FromBody] RequestRegistrarUsuarioJson request)
    {
        var resultado = await useCase.Executar(request);
        return Created(string.Empty, resultado);
    }

    [HttpPut]
    [Route("alterar-senha")]
    [ProducesResponseType( StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public async Task<IActionResult> AlterarSenha([FromServices] IAlterarSenhaUseCase useCase,
        [FromBody] RequestAlterarSenhaJson request)
    {
        await useCase.Executar(request);
        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public async Task<IActionResult> AlterarUsuario([FromServices] IAlterarUsuarioUseCase useCase,
       [FromBody] RequestAlterarUsuarioJson request)
    {
        await useCase.Executar(request);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponsePerfilUsuarioJson), StatusCodes.Status200OK)]
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public async Task<IActionResult> RecuperarPerfil([FromServices] IRecuperarPerfilUseCase useCase)
    {
        var resultado = await useCase.Executar();
        return Ok(resultado);
    }
}