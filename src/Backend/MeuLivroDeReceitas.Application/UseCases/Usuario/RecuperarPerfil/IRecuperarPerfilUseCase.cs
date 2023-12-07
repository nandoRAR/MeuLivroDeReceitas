using MeuLivroDeReceitas.Comunicacao.Response;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.RecuperarPerfil;
public interface IRecuperarPerfilUseCase
{
    Task<ResponsePerfilUsuarioJson> Executar();
}
