using System.Security.Cryptography;
using System.Text;

namespace MeuLivroDeReceitas.Application.Servicos.Criptografria;

public class EncriptadorDeSenha
{
    private readonly string _chaveAdicional;

    public EncriptadorDeSenha(string chaveAdicional)
    {
        _chaveAdicional = chaveAdicional;
    }

    public string Criptografar(string senha)
    {
        var senhaComChaveAdcional = $"{senha}{_chaveAdicional}";
        var bytes = Encoding.UTF8.GetBytes(senhaComChaveAdcional);
        var sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(bytes);
        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
}
