using ValidarMaoDePoker.Domain;

namespace ValidarMaoDePoker.Tests;

[TestClass]
public sealed class ClassificadorMaoPokerTests
{
    private readonly ClassificadorMaoPoker classificador = new();

    [TestMethod]
    [DataRow("2C 5P 7E 9O KO", ClassificacaoMaoPoker.CartaAlta)]
    [DataRow("2C 2P 7E 9O KO", ClassificacaoMaoPoker.UmPar)]
    [DataRow("2C 2P 7E 7O KO", ClassificacaoMaoPoker.DoisPares)]
    [DataRow("2C 2P 2E 9O KO", ClassificacaoMaoPoker.Trinca)]
    [DataRow("2C 3P 4E 5O 6C", ClassificacaoMaoPoker.Sequencia)]
    [DataRow("2C 5C 7C 9C KC", ClassificacaoMaoPoker.Flush)]
    [DataRow("2C 2P 2E 9O 9C", ClassificacaoMaoPoker.FullHouse)]
    [DataRow("2C 2P 2E 2O KC", ClassificacaoMaoPoker.Quadra)]
    [DataRow("2C 3C 4C 5C 6C", ClassificacaoMaoPoker.StraightFlush)]
    [DataRow("DC JC QC KC AC", ClassificacaoMaoPoker.RoyalFlush)]
    public void Classificar_RetornaClassificacaoEsperada(string texto, ClassificacaoMaoPoker esperada)
    {
        Assert.AreEqual(esperada, classificador.Classificar(MaoPoker.Parse(texto)));
    }

    [TestMethod]
    [DataRow("2C 2P 4E 5O 6C", ClassificacaoMaoPoker.UmPar)]
    [DataRow("2C 2P 2E 5O 6C", ClassificacaoMaoPoker.Trinca)]
    [DataRow("3C 4P 4E 5O 7C", ClassificacaoMaoPoker.UmPar)]
    public void Classificar_ValoresRepetidosNaoFormamSequencia(
        string texto,
        ClassificacaoMaoPoker classificacaoEsperada)
    {
        Assert.AreEqual(classificacaoEsperada, classificador.Classificar(MaoPoker.Parse(texto)));
    }

    [TestMethod]
    [DataRow("DO JO QO KO AO")]
    [DataRow("DC JC QC KC AC")]
    [DataRow("DE JE QE KE AE")]
    [DataRow("DP JP QP KP AP")]
    public void Classificar_IdentificaRoyalFlushEmTodosOsNaipes(string texto)
    {
        Assert.AreEqual(
            ClassificacaoMaoPoker.RoyalFlush,
            classificador.Classificar(MaoPoker.Parse(texto)));
    }

    [TestMethod]
    public void Classificar_AceDoisTresQuatroCincoNaoFormaSequencia()
    {
        var classificacao = classificador.Classificar(MaoPoker.Parse("AC 2P 3E 4O 5C"));

        Assert.AreEqual(ClassificacaoMaoPoker.CartaAlta, classificacao);
    }

    [TestMethod]
    public void Classificar_FlushSemSequenciaNaoEhStraightFlush()
    {
        var classificacao = classificador.Classificar(MaoPoker.Parse("2C 5C 7C 9C KC"));

        Assert.AreEqual(ClassificacaoMaoPoker.Flush, classificacao);
    }

    [TestMethod]
    public void Classificar_SequenciaComNaipesDiferentesNaoEhFlush()
    {
        var classificacao = classificador.Classificar(MaoPoker.Parse("2C 3P 4E 5O 6C"));

        Assert.AreEqual(ClassificacaoMaoPoker.Sequencia, classificacao);
    }

    [TestMethod]
    public void Classificar_StraightFlushNaoRoyalNaoEhRoyalFlush()
    {
        var classificacao = classificador.Classificar(MaoPoker.Parse("6C 7C 8C 9C DC"));

        Assert.AreEqual(ClassificacaoMaoPoker.StraightFlush, classificacao);
    }

    [TestMethod]
    public void Classificar_MaoComFullHouseNaoEhTrincaOuDoisPares()
    {
        var classificacao = classificador.Classificar(MaoPoker.Parse("5C 5P 5E 7O 7C"));

        Assert.AreEqual(ClassificacaoMaoPoker.FullHouse, classificacao);
    }

    [TestMethod]
    public void Classificar_MaoComQuadraNaoEhTrincaOuDoisPares()
    {
        var classificacao = classificador.Classificar(MaoPoker.Parse("5C 5P 5E 5O 7C"));

        Assert.AreEqual(ClassificacaoMaoPoker.Quadra, classificacao);
    }

    [TestMethod]
    [DataRow("2C 3P 4E 5O 6C")]
    [DataRow("6C 2C 5O 3P 4E")]
    [DataRow("4E 6C 3P 2C 5O")]
    public void Classificar_IndependeDaOrdemDasCartas(string texto)
    {
        Assert.AreEqual(
            ClassificacaoMaoPoker.Sequencia,
            classificador.Classificar(MaoPoker.Parse(texto)));
    }

    [TestMethod]
    [DataRow("2C 5P 7E 9O KO", "KO 9O 7E 5P 2C", ClassificacaoMaoPoker.CartaAlta)]
    [DataRow("2C 2P 7E 9O KO", "KO 9O 7E 2P 2C", ClassificacaoMaoPoker.UmPar)]
    [DataRow("2C 2P 7E 7O KO", "KO 7O 7E 2P 2C", ClassificacaoMaoPoker.DoisPares)]
    [DataRow("2C 2P 2E 9O KO", "KO 9O 2E 2P 2C", ClassificacaoMaoPoker.Trinca)]
    [DataRow("2C 3P 4E 5O 6C", "6C 5O 4E 3P 2C", ClassificacaoMaoPoker.Sequencia)]
    [DataRow("2C 5C 7C 9C KC", "KC 9C 7C 5C 2C", ClassificacaoMaoPoker.Flush)]
    [DataRow("2C 2P 2E 9O 9C", "9C 9O 2E 2P 2C", ClassificacaoMaoPoker.FullHouse)]
    [DataRow("2C 2P 2E 2O KC", "KC 2O 2E 2P 2C", ClassificacaoMaoPoker.Quadra)]
    [DataRow("2C 3C 4C 5C 6C", "6C 5C 4C 3C 2C", ClassificacaoMaoPoker.StraightFlush)]
    [DataRow("DC JC QC KC AC", "AC KC QC JC DC", ClassificacaoMaoPoker.RoyalFlush)]
    public void Classificar_OrdemDasCartasNaoAlteraClassificacao(
        string ordemOriginal,
        string ordemInvertida,
        ClassificacaoMaoPoker esperada)
    {
        Assert.AreEqual(esperada, classificador.Classificar(MaoPoker.Parse(ordemOriginal)));
        Assert.AreEqual(esperada, classificador.Classificar(MaoPoker.Parse(ordemInvertida)));
    }

    [TestMethod]
    [DataRow(ClassificacaoMaoPoker.CartaAlta, "Carta Alta")]
    [DataRow(ClassificacaoMaoPoker.UmPar, "Um Par")]
    [DataRow(ClassificacaoMaoPoker.DoisPares, "Dois Pares")]
    [DataRow(ClassificacaoMaoPoker.Trinca, "Trinca")]
    [DataRow(ClassificacaoMaoPoker.Sequencia, "Sequência")]
    [DataRow(ClassificacaoMaoPoker.Flush, "Flush")]
    [DataRow(ClassificacaoMaoPoker.FullHouse, "Full House")]
    [DataRow(ClassificacaoMaoPoker.Quadra, "Quadra")]
    [DataRow(ClassificacaoMaoPoker.StraightFlush, "Straight Flush")]
    [DataRow(ClassificacaoMaoPoker.RoyalFlush, "Royal Flush")]
    public void NomeEmPortugues_DisponibilizaNomeDaClassificacao(
        ClassificacaoMaoPoker classificacao,
        string nomeEsperado)
    {
        Assert.AreEqual(nomeEsperado, classificacao.NomeEmPortugues());
    }

    [TestMethod]
    public void Classificar_RejeitaMaoNula()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => classificador.Classificar(null!));
    }

    [TestMethod]
    public void Classificacao_ValoresNumericosRepresentamRankingCrescente()
    {
        var classificacoes = Enum.GetValues<ClassificacaoMaoPoker>();

        CollectionAssert.AreEqual(
            Enumerable.Range(1, 10).ToArray(),
            classificacoes.Select(classificacao => (int)classificacao).ToArray());
    }
}
