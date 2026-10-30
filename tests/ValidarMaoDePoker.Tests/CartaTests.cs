using ValidarMaoDePoker.Domain;

namespace ValidarMaoDePoker.Tests;

[TestClass]
public sealed class CartaTests
{
    [TestMethod]
    [DataRow("2", ValorCarta.Dois)]
    [DataRow("3", ValorCarta.Tres)]
    [DataRow("4", ValorCarta.Quatro)]
    [DataRow("5", ValorCarta.Cinco)]
    [DataRow("6", ValorCarta.Seis)]
    [DataRow("7", ValorCarta.Sete)]
    [DataRow("8", ValorCarta.Oito)]
    [DataRow("9", ValorCarta.Nove)]
    [DataRow("D", ValorCarta.Dez)]
    [DataRow("J", ValorCarta.Valete)]
    [DataRow("Q", ValorCarta.Dama)]
    [DataRow("K", ValorCarta.Rei)]
    [DataRow("A", ValorCarta.As)]
    public void Parse_ConverteTodosOsValores(string simbolo, ValorCarta valorEsperado)
    {
        var carta = Carta.Parse(simbolo + "C");

        Assert.AreEqual(valorEsperado, carta.Valor);
        Assert.AreEqual(simbolo + "C", carta.ToString());
    }

    [TestMethod]
    [DataRow("O", Naipe.Ouros)]
    [DataRow("C", Naipe.Copas)]
    [DataRow("E", Naipe.Espadas)]
    [DataRow("P", Naipe.Paus)]
    public void Parse_ConverteTodosOsNaipes(string simbolo, Naipe naipeEsperado)
    {
        var carta = Carta.Parse("2" + simbolo);

        Assert.AreEqual(naipeEsperado, carta.Naipe);
        Assert.AreEqual("2" + simbolo, carta.ToString());
    }

    [TestMethod]
    public void CartasComMesmoValorENaipeSaoIguais()
    {
        var primeira = Carta.Parse("5C");
        var segunda = new Carta(ValorCarta.Cinco, Naipe.Copas);

        Assert.AreEqual(primeira, segunda);
    }

    [TestMethod]
    public void Construtor_RejeitaValorDeEnumInvalido()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new Carta((ValorCarta)1, Naipe.Copas));
    }

    [TestMethod]
    public void Construtor_RejeitaNaipeDeEnumInvalido()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new Carta(ValorCarta.As, (Naipe)4));
    }

    [TestMethod]
    public void AsValeQuatorze()
    {
        Assert.AreEqual(14, (int)Carta.Parse("AC").Valor);
    }

    [TestMethod]
    public void Parse_RejeitaValorInvalido()
    {
        Assert.ThrowsExactly<FormatException>(() => Carta.Parse("1C"));
    }

    [TestMethod]
    public void Parse_RejeitaNaipeInvalido()
    {
        Assert.ThrowsExactly<FormatException>(() => Carta.Parse("7S"));
    }

    [TestMethod]
    [DataRow("10O")]
    [DataRow("5")]
    [DataRow("5CC")]
    public void Parse_RejeitaFormatoInvalido(string texto)
    {
        Assert.ThrowsExactly<FormatException>(() => Carta.Parse(texto));
    }
}
