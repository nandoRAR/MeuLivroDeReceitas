using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaOsTestes.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "Rl0+WUxZbntxVic1O1lGVWsydTosTDU6TjVsYm80e2c9OzFPcyE5Xw==");
    }
}
