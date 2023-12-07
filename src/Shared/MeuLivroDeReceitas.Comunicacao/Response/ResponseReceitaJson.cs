using MeuLivroDeReceitas.Comunicacao.Enum;

namespace MeuLivroDeReceitas.Comunicacao.Response;
public class ResponseReceitaJson
{
    public string Id { get; set; }
    public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }
    public int TempoPreparo { get; set; }
    public List<ResponseIngredienteJson> Ingredientes { get; set; }
}
