namespace MeuLivroDeReceitas.Comunicacao.Response;

public class ResponseErroJson
{
    public List<string> Mensagens { get; set; }

    public ResponseErroJson(string mensagem)
    {
        Mensagens = new List<string> { mensagem };
    }

    public ResponseErroJson(List<string> mensagens)
    {
        Mensagens = mensagens;
    }
}
