using MeuLivroDeReceitas.Comunicacao.Request;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
public interface IAtualizarReceitaUseCase
{
    Task Executar(long id, RequestReceitaJson request);
}
