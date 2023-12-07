using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Deletar;
public class DeletarReceitaUseCase : IDeletarReceitaUseCase
{
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IReceitaWriteOnlyRepositorio _repositorioWriteOnly;
    private readonly IReceitaReadOnlyRepositorio _repositorioReadOnly;

    public DeletarReceitaUseCase(IUnidadeDeTrabalho unidadeDeTrabalho, IUsuarioLogado usuarioLogado,
        IReceitaWriteOnlyRepositorio repositorioWriteOnly,
        IReceitaReadOnlyRepositorio repositorioReadOnly)
    {
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _usuarioLogado = usuarioLogado;
        _repositorioWriteOnly = repositorioWriteOnly;
        _repositorioReadOnly = repositorioReadOnly;
    }
    public async Task Executar(long id)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receita = await _repositorioReadOnly.RecuperarPorId(id);

        Validar(usuarioLogado, receita);

        await _repositorioWriteOnly.Deletar(id);

        await _unidadeDeTrabalho.Commit();
    }

    private static void Validar(Domain.Entidades.Usuario usuarioLogado, Domain.Entidades.Receita receita)
    {
        if (receita is null || receita.UsuarioId != usuarioLogado.Id)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
        }
    }
}
