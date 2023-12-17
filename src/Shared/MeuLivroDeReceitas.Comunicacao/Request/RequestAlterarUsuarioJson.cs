using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Comunicacao.Request;
public class RequestAlterarUsuarioJson
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
}
