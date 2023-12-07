using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Comunicacao.Response;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
public class RegistrarReceitaUseCase : IRegistrarReceitaUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IReceitaWriteOnlyRepositorio _repositorio;

    public RegistrarReceitaUseCase(IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho, IUsuarioLogado usuarioLogado,
        IReceitaWriteOnlyRepositorio repositorio)
    {
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _usuarioLogado = usuarioLogado;
        _repositorio = repositorio;
    }

    public async Task<ResponseReceitaJson> Executar(RequestReceitaJson request)
    {
        Validar(request);
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receita = _mapper.Map<Domain.Entidades.Receita>(request);
        receita.UsuarioId = usuarioLogado.Id;

        await _repositorio.Registrar(receita);
        await _unidadeDeTrabalho.Commit();

        return _mapper.Map<ResponseReceitaJson>(receita);
    }

    private static void Validar(RequestReceitaJson request)
    {
        var validator = new RegistrarReceitaValidator();
        var resultado = validator.Validate(request);
         
        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }
    }
}
