using MeuLivroDeReceitas.Application.Servicos.Criptografria;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;

public class AlterarSenhaUseCase : IAlterarSenhaUseCase
{
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUsuarioUpdateOnlyRepositorio _repositorio;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    
    public AlterarSenhaUseCase(IUsuarioUpdateOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado,
        EncriptadorDeSenha encriptadorDeSenha, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _encriptadorDeSenha = encriptadorDeSenha;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Executar(RequestAlterarSenhaJson request)
    {
        var usuarioLogado =  await _usuarioLogado.RecuperarUsuario();
        var usuario = await _repositorio.RecuperarPorId(usuarioLogado.Id);

        Validar(request, usuario);
        usuario.Senha = _encriptadorDeSenha.Criptografar(request.NovaSenha);

        _repositorio.Update(usuario);
        await _unidadeDeTrabalho.Commit();
    }

    private void Validar(RequestAlterarSenhaJson request, Domain.Entidades.Usuario usuario)
    {
        var validator = new AlterarSenhaValidator();
        var resultado = validator.Validate(request);

        var senhaAtualCriptografada = _encriptadorDeSenha.Criptografar(request.SenhaAtual);

        if (!usuario.Senha.Equals(senhaAtualCriptografada))
        {
            resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("senhaAtual", 
                ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));
        }

        if (!resultado.IsValid) 
        {
            var mensagens = resultado.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagens);
        }
    }
}
