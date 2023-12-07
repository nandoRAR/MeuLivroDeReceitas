using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Request;
using MeuLivroDeReceitas.Comunicacao.Response;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Application.UseCases.Dashboard;
public class DashboardUseCase : IDashboardUseCase
{
    private readonly IConexaoReadOnlyRepositorio _conexaorepositorio;
    private readonly IReceitaReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;

    public DashboardUseCase(IReceitaReadOnlyRepositorio repositorio, 
        IUsuarioLogado usuarioLogado,
        IMapper mapper,
         IConexaoReadOnlyRepositorio conexaorepositorio)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
        _conexaorepositorio = conexaorepositorio;
    }

    public async Task<ResponseDashboardJson> Executar(RequestDashboardJson request)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receitas = await _repositorio.RecuperarTodasDoUsuario(usuarioLogado.Id);

        receitas = Filtrar(request, receitas);

        var receitasUsuariosConctados = await ReceitasUsuariosConectados(request, usuarioLogado);

        receitas = receitas.Concat(receitasUsuariosConctados).ToList();

        return new ResponseDashboardJson
        {
            Receitas = _mapper.Map<List<ResponseReceitaDashboardJson>>(receitas)
        };
    }

    private async Task<IList<Domain.Entidades.Receita>> ReceitasUsuariosConectados(RequestDashboardJson request, 
        Domain.Entidades.Usuario usuarioLogado)
    {
        var conexoes = await _conexaorepositorio.RecuperarDoUsuario(usuarioLogado.Id);

        var usuariosConectados = conexoes.Select(c => c.Id).ToList();
        var receitasUsuariosConectados = await _repositorio.RecuperarTodasDosUsuarios(usuariosConectados);

        return  Filtrar(request, receitasUsuariosConectados);
    }

        private static IList<Domain.Entidades.Receita> Filtrar(RequestDashboardJson request, IList<Domain.Entidades.Receita> receitas)
    {
        if (receitas is null)
            return new List<Domain.Entidades.Receita>();

        var receitasFiltradas = receitas;

        if (request.Categoria.HasValue) 
        {
            receitasFiltradas = receitas.Where(r => r.Categoria == (Domain.Enum.Categoria)request.Categoria.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(request.TituloOuIngrediente))
        {
            receitasFiltradas = receitas.Where(r => r.Titulo.CompararSemConsiderarAcentoUpperCase(request.TituloOuIngrediente) || 
                r.Ingredientes.Any(ingrediente => ingrediente.Produto.CompararSemConsiderarAcentoUpperCase(request.TituloOuIngrediente)))
                .ToList();
        }

        return receitasFiltradas.OrderBy(c => c.Titulo).ToList();
    }
}
