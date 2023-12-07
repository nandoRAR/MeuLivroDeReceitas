using MeuLivroDeReceitas.Comunicacao.Response;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
public interface IRecuperarReceitaPorIdUseCase
{
    Task<ResponseReceitaJson> Executar(long id);
}
