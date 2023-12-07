using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Dashboard;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Comunicacao.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class DashboardController : MeuLivroDeReceitasController
{
    [HttpPut]
    [ProducesResponseType(typeof(ResponseDashboardJson), StatusCodes.Status200OK)]
    [ProducesResponseType( StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecuperarDashboard([FromServices] IDashboardUseCase useCase,
        [FromBody] RequestDashboardJson request)
    {
        var resultado = await useCase.Executar(request);

        if (resultado.Receitas.Any())
        {
            return Ok(resultado);
        }
        return NoContent();
    }
}