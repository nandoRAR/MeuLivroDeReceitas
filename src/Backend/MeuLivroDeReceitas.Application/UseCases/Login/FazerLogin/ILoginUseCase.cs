using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Comunicacao.Response;

namespace MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;

public interface ILoginUseCase
{
    Task<ResponseLoginJson> Executar(RequestLoginJson request);
}
