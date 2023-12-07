using Microsoft.AspNetCore.SignalR;
using System.Timers;

namespace MeuLivroDeReceitas.Api.WebSockets;

public class Conexao
{
    private readonly IHubContext<AdicionarConexao> _hubContext;
    private readonly string UsuarioQueCriouQRCodeConnectionId;

    private Action<string> _callbackTempoExpirado;
    private string ConnectionIdUsuarioLeitorQRCode;

    public Conexao(IHubContext<AdicionarConexao> hubContext, string usuarioQueCriouQRCodeConnectionId)
    {
        _hubContext = hubContext;
        UsuarioQueCriouQRCodeConnectionId = usuarioQueCriouQRCodeConnectionId;
    }

    private short tempoRestanteSegundos { get; set; }
    private System.Timers.Timer Timer { get; set; }

    public void IniciarContagemTempo(Action<string> callbackTempoExpirado)
    {
        _callbackTempoExpirado = callbackTempoExpirado;
        StartTimer();
    }

    public void ResetarContagemTempo()
    {
        StopTimer();
        StartTimer();
    }

    public void StopTimer()
    {
        Timer?.Stop();
        Timer?.Dispose();
        Timer = null;
    }
    private void StartTimer()
    {
        tempoRestanteSegundos = 60;
        Timer = new System.Timers.Timer(1000)
        {
            Enabled = false
        };
        Timer.Elapsed += ElapsedTimer;
        Timer.Enabled = true;
    }

    public void SetConnectionIdUsuarioLeitorQRCode(string connectionId)
    {
        ConnectionIdUsuarioLeitorQRCode = connectionId;
    }

    public string UsuarioQueLeuQRCode()
    {
        return ConnectionIdUsuarioLeitorQRCode;
    }

    private async void ElapsedTimer(object sender, ElapsedEventArgs e)
    {
        if (tempoRestanteSegundos >= 0)
            await _hubContext.Clients.Client(UsuarioQueCriouQRCodeConnectionId).SendAsync("SetTempoRestante", tempoRestanteSegundos--);
        else
        {
            StopTimer();
            _callbackTempoExpirado(UsuarioQueCriouQRCodeConnectionId);
        }
    }
}
