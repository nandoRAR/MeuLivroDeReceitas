using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Comunicacao.Response;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
public interface IRegistrarReceitaUseCase
{
    Task<ResponseReceitaJson> Executar(RequestReceitaJson request);
}
