using AutoMapper;
using HashidsNet;

namespace MeuLivroDeReceitas.Application.Servicos.AutoMapper;

public class AutoMapperConfiguracao : Profile
{
    private readonly IHashids _hashids;

    public AutoMapperConfiguracao(IHashids hashids)
    {
        _hashids = hashids;

        RequisicaoParaEntidade();
        EntidadeParaResposta();
    }

    private void RequisicaoParaEntidade()
    {
        CreateMap<Comunicacao.Request.RequestRegistrarUsuarioJson, Domain.Entidades.Usuario>()
           .ForMember(destino => destino.Senha, config => config.Ignore());

        CreateMap<Comunicacao.Request.RequestReceitaJson, Domain.Entidades.Receita>();
        CreateMap<Comunicacao.Request.RequestIngredienteJson, Domain.Entidades.Ingrediente>();
    }

    private void EntidadeParaResposta()
    {
        CreateMap<Domain.Entidades.Receita, Comunicacao.Response.ResponseReceitaJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));

        CreateMap<Domain.Entidades.Ingrediente, Comunicacao.Response.ResponseIngredienteJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));

        CreateMap<Domain.Entidades.Receita, Comunicacao.Response.ResponseReceitaDashboardJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)))
            .ForMember(destino => destino.QuantidadeIngredientes, config => config.MapFrom(origem => origem.Ingredientes.Count));

        CreateMap<Domain.Entidades.Usuario, Comunicacao.Response.ResponsePerfilUsuarioJson>();

        CreateMap<Domain.Entidades.Usuario, Comunicacao.Response.ResponseUsuarioConcectadoJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));
    }
}
