using Bogus;
using MeuLivroDeReceitas.Comunicacao.Response;
using Utilitario.ParaOsTestes.Hashids;

namespace Utilitario.ParaOsTestes.Response;
public class ResponseUsuarioConexaoBuilder
{
    public static ResponseUsuarioConexaoJson Construir()
    {
        var hashIds = HashidsBuilder.Instance().Build();

        return new Faker<ResponseUsuarioConexaoJson>()
            .RuleFor(c => c.Id, f => hashIds.EncodeLong(f.Random.Long(1, 5000)))
            .RuleFor(c => c.Nome, f => f.Person.FullName);
    }
}
