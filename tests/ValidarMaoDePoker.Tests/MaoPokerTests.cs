using ValidarMaoDePoker.Domain;

namespace ValidarMaoDePoker.Tests;

[TestClass]
public sealed class MaoPokerTests
{
    [TestMethod]
    public void Parse_CriaMaoValidaComCincoCartas()
    {
        var mao = MaoPoker.Parse("5C 5P 6E 7P KO");

        Assert.HasCount(5, mao.Cartas);
        CollectionAssert.AreEqual(
            new[] { "5C", "5P", "6E", "7P", "KO" },
            mao.Cartas.Select(carta => carta.ToString()).ToArray());
    }

    [TestMethod]
    public void Parse_AceitaEspacosAdicionaisAoRedorEDentroDaEntrada()
    {
        var mao = MaoPoker.Parse("  5C   5P  6E 7P   KO  ");

        Assert.HasCount(5, mao.Cartas);
    }

    [TestMethod]
    [DataRow("5C 5P 6E 7P")]
    [DataRow("5C 5P 6E 7P KO 2O")]
    public void Parse_RejeitaQuantidadeIncorretaDeCartas(string texto)
    {
        Assert.ThrowsExactly<ArgumentException>(() => MaoPoker.Parse(texto));
    }

    [TestMethod]
    public void Parse_RejeitaCartaDuplicada()
    {
        Assert.ThrowsExactly<ArgumentException>(() => MaoPoker.Parse("5C 5C 6E 7P KO"));
    }

    [TestMethod]
    public void Parse_RejeitaValorInvalido()
    {
        Assert.ThrowsExactly<FormatException>(() => MaoPoker.Parse("1C 5P 6E 7P KO"));
    }

    [TestMethod]
    public void Parse_RejeitaNaipeInvalido()
    {
        Assert.ThrowsExactly<FormatException>(() => MaoPoker.Parse("5C 5P 6E 7S KO"));
    }

    [TestMethod]
    public void Parse_RejeitaFormatoInvalido()
    {
        Assert.ThrowsExactly<FormatException>(() => MaoPoker.Parse("10O 5P 6E 7P KO"));
    }

    [TestMethod]
    public void Parse_RejeitaEntradaNula()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => MaoPoker.Parse(null));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Parse_RejeitaEntradaVazia(string texto)
    {
        Assert.ThrowsExactly<ArgumentException>(() => MaoPoker.Parse(texto));
    }

    [TestMethod]
    public void Mao_CopiaCartasParaEvitarAlteracoesExternas()
    {
        var cartas = new List<Carta>
        {
            Carta.Parse("5C"),
            Carta.Parse("5P"),
            Carta.Parse("6E"),
            Carta.Parse("7P"),
            Carta.Parse("KO")
        };
        var mao = new MaoPoker(cartas);

        cartas.Clear();

        Assert.HasCount(5, mao.Cartas);
    }

    [TestMethod]
    public void Construtor_RejeitaCartaNula()
    {
        var cartas = new Carta[]
        {
            default!,
            Carta.Parse("5P"),
            Carta.Parse("6E"),
            Carta.Parse("7P"),
            Carta.Parse("KO")
        };

        Assert.ThrowsExactly<ArgumentException>(() => new MaoPoker(cartas));
    }

    [TestMethod]
    public void Mao_ImpedeAlteracaoDaColecaoExposta()
    {
        var mao = MaoPoker.Parse("5C 5P 6E 7P KO");
        var cartas = (IList<Carta>)mao.Cartas;

        Assert.ThrowsExactly<NotSupportedException>(() => cartas.Clear());
    }
}
