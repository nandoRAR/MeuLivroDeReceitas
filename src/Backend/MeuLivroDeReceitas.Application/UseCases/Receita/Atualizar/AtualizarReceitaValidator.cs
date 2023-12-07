using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Request;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
public class AtualizarReceitaValidator : AbstractValidator<RequestReceitaJson>
{
    public AtualizarReceitaValidator()
    {
        RuleFor(x => x).SetValidator(new ReceitaValidator());
    }

}
