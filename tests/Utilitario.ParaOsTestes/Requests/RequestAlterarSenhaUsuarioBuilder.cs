using Bogus;
using MeuLivroDeReceitas.Comunicacao.Request;

namespace Utilitario.ParaOsTestes.Requests;

public class RequestAlterarSenhaUsuarioBuilder
{
    public static RequestAlterarSenhaJson Construir(int tamanhoSenha = 10)
    {
        return new Faker<RequestAlterarSenhaJson>()
            .RuleFor(c => c.SenhaAtual, f => f.Internet.Password(10))
            .RuleFor(c => c.NovaSenha, f => f.Internet.Password(tamanhoSenha));
    }

}
