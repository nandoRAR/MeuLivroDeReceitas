using MeuLivroDeReceitas.Application.Servicos.Criptografria;

namespace Utilitario.ParaOsTestes.Criptografria;

public class EncriptadorDeSenhaBuilder
{
    public static EncriptadorDeSenha Instancia()
    {
        return new EncriptadorDeSenha("ABCD123");
    }
}
