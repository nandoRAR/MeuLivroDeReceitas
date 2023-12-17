using MeuLivroDeReceitas.Application.Servicos.Criptografria;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Domain.Repositorios;
using AutoMapper;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarUsuario;
public class AlterarUsuarioUseCase : IAlterarUsuarioUseCase
{
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUsuarioUpdateOnlyRepositorio _repositorio;
    private readonly IUsuarioReadOnlyRepositorio _usuarioReadOnlyRepositorio;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IMapper _mapper;

    public AlterarUsuarioUseCase(IUsuarioUpdateOnlyRepositorio repositorio,
        IUsuarioReadOnlyRepositorio usuarioReadOnlyRepositorio,
        IUsuarioLogado usuarioLogado,
        IUnidadeDeTrabalho unidadeDeTrabalho,
        IMapper mapper)
    {
        _repositorio = repositorio;
        _usuarioReadOnlyRepositorio = usuarioReadOnlyRepositorio;
        _usuarioLogado = usuarioLogado;     
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _mapper = mapper;
    }
    public async Task Executar(RequestAlterarUsuarioJson request)
    {

        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var usuario = await _repositorio.RecuperarPorId(usuarioLogado.Id);

        await Validar(request, usuario);

        _mapper.Map(request, usuario);

        _repositorio.Update(usuario);
        await _unidadeDeTrabalho.Commit();

    }

    private async Task Validar(RequestAlterarUsuarioJson request, Domain.Entidades.Usuario usuario)
    {
        var validator = new AlterarUsuarioValidator();
        var resultado = validator.Validate(request);

        var existeUsuarioComEmail = await _usuarioReadOnlyRepositorio.ExisteUsuarioComEmail(request.Email);

        if (existeUsuarioComEmail && request.Email != usuario.Email)
        {
            resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMensagensDeErro.EMAIL_JA_CADASTRADO));
        }

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }
    }
}
