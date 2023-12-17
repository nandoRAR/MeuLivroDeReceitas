using MeuLivroDeReceitas.Comunicacao.Request;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarUsuario;
public interface IAlterarUsuarioUseCase
{
    Task Executar(RequestAlterarUsuarioJson request);
}
