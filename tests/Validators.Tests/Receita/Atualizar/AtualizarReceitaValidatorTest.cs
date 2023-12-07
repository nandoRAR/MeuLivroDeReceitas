﻿using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaOsTestes.Requests;
using Xunit;

namespace Validators.Tests.Receita.Atualizar;
public class AtualizarReceitaValidatorTest
{
    [Fact]
    public void Validar_Sucesso()
    {
        var validator = new AtualizarReceitaValidator();

        var requisicao = RequestReceitaBuilder.Construir();

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validar_Erro_Categoria_Invalida()
    {
        var validator = new AtualizarReceitaValidator();

        var requisicao = RequestReceitaBuilder.Construir();
        requisicao.Categoria = (Categoria)1000;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.CATEGORIA_RECEITA_INVALIDA));
    }

    [Fact]
    public void Validar_Erro_ModoPreparo_Vazio()
    {
        var validator = new AtualizarReceitaValidator();

        var requisicao = RequestReceitaBuilder.Construir();
        requisicao.ModoPreparo = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.MODOPREPARO_RECEITA_EMBRANCO));
    }

    [Fact]
    public void Validar_Erro_ListaIngredientes_Vazio()
    {
        var validator = new AtualizarReceitaValidator();

        var requisicao = RequestReceitaBuilder.Construir();
        requisicao.Ingredientes.Clear();

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE));
    }

    [Fact]
    public void Validar_Erro_Produto_Ingrediente_Vazio()
    {
        var validator = new AtualizarReceitaValidator();

        var requisicao = RequestReceitaBuilder.Construir();
        requisicao.Ingredientes.First().Produto = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.RECEITA_INGREDIENTE_PRODUTO_EMBRANCO));
    }

    [Fact]
    public void Validar_Erro_Quantidade_Ingrediente_Vazio()
    {
        var validator = new AtualizarReceitaValidator();

        var requisicao = RequestReceitaBuilder.Construir();
        requisicao.Ingredientes.First().Quantidade = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.RECEITA_INGREDIENTE_QUANTIDADE_EMBRANCO));
    }

    [Fact]
    public void Validar_Erro_Ingrediente_Repetido()
    {
        var validator = new AtualizarReceitaValidator();

        var requisicao = RequestReceitaBuilder.Construir();
        requisicao.Ingredientes.Add(requisicao.Ingredientes.First());

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.RECEITA_INGREDIENTES_REPETIDOS));
    }
}
