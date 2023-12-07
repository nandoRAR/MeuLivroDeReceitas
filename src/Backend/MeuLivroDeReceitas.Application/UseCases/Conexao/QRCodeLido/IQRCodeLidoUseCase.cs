using MeuLivroDeReceitas.Comunicacao.Response;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
public interface IQRCodeLidoUseCase
{
    Task<(ResponseUsuarioConexaoJson usuarioParaSeConectar, string idUsuarioQueGerouQRCode)> Executar(string codigoConexao);
}
