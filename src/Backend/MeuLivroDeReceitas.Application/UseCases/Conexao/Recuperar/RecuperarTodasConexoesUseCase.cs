using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Response;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.Recuperar;
public class RecuperarTodasConexoesUseCase : IRecuperarTodasConexoesUseCase
{
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IReceitaReadOnlyRepositorio _repositorioReceita;
    private readonly IConexaoReadOnlyRepositorio _repositorio;
    private readonly IMapper _mapper;

    public RecuperarTodasConexoesUseCase(IUsuarioLogado usuarioLogado,
         IConexaoReadOnlyRepositorio repositorio,
         IMapper mapper,
         IReceitaReadOnlyRepositorio repositorioReceita)
    {
        _usuarioLogado = usuarioLogado;
        _repositorio = repositorio;
        _mapper = mapper;
        _repositorioReceita = repositorioReceita;
    }

    public async Task<ResponseConexoesDoUsuarioJson> Executar()
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var conexoes = await _repositorio.RecuperarDoUsuario(usuarioLogado.Id);

        var tarefas = conexoes.Select(async usuario =>
        {
            var quantidadeReceitas = await _repositorioReceita.QuantidadeReceitas(usuario.Id);

            var usuarioJson = _mapper.Map<ResponseUsuarioConcectadoJson>(usuario);
            usuarioJson.QuantidadeReceitas = quantidadeReceitas;

            return usuarioJson;
        });

        return new ResponseConexoesDoUsuarioJson
        {
            Usuarios = await Task.WhenAll(tarefas)
        };
    }
}
