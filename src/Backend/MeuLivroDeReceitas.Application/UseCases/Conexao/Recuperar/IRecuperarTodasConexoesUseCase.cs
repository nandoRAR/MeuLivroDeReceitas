using MeuLivroDeReceitas.Comunicacao.Response;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.Recuperar;
public interface IRecuperarTodasConexoesUseCase
{
    Task<ResponseConexoesDoUsuarioJson> Executar();
}
