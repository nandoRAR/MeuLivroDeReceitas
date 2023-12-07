using Bogus;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Request;

namespace Utilitario.ParaOsTestes.Requests;
public class RequestReceitaBuilder
{
    public static RequestReceitaJson Construir()
    {
        return new Faker<RequestReceitaJson>()
            .RuleFor(c => c.Titulo, f => f.Commerce.Department())
            .RuleFor(c => c.Categoria, f => f.PickRandom<Categoria>())
            .RuleFor(c => c.ModoPreparo, f => f.Lorem.Paragraph())
            .RuleFor(c => c.Ingredientes, f => RandomIngredientes(f))
            .RuleFor(c => c.TempoPreparo, f => f.Random.Int(1, 1000));
    }

    private static List<RequestIngredienteJson> RandomIngredientes(Faker f)
    {
        List<RequestIngredienteJson> ingredientes = new();

        for (int i = 0; i < f.Random.Int(1, 10); i++)
        {
            ingredientes.Add(new RequestIngredienteJson
            {
                Produto = f.Commerce.ProductName(),
                Quantidade = $"{f.Random.Double(1, 10)} {f.Random.Word()}"
            });
        }

        return ingredientes;
    }
}
