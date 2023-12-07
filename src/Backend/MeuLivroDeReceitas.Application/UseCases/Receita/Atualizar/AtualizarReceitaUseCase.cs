using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Domain.Repositorios;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
public class AtualizarReceitaUseCase : IAtualizarReceitaUseCase
{

    private readonly IReceitaUpdateOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public AtualizarReceitaUseCase(IReceitaUpdateOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado,
        IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Executar(long id, RequestReceitaJson request)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receita = await _repositorio.RecuperarPorId(id);

        Validar(usuarioLogado, receita, request);

        _mapper.Map(request, receita);

        _repositorio.Update(receita);

        await _unidadeDeTrabalho.Commit();

    }

    private static void Validar(Domain.Entidades.Usuario usuarioLogado, Domain.Entidades.Receita receita,
        RequestReceitaJson request)
    {
        if (receita is null || receita.UsuarioId != usuarioLogado.Id)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
        }

        var validator = new AtualizarReceitaValidator();
        var resultado = validator.Validate(request);

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }
    }
}
