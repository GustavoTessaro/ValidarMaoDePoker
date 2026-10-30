using ValidarMaoDePoker.Domain;

namespace ValidarMaoDePoker.Tests;

[TestClass]
public sealed class ComparadorMaoPokerTests
{
    private readonly ComparadorMaoPoker comparador = new();

    [TestMethod]
    [DataRow(
        "5C 5P 6E 7P KO", "2P 3E 8E 8O DO",
        ClassificacaoMaoPoker.UmPar, ClassificacaoMaoPoker.UmPar, VencedorComparacao.Jogador2)]
    [DataRow(
        "5O 8P 9E JE AP", "2P 5P 7O 8E QC",
        ClassificacaoMaoPoker.CartaAlta, ClassificacaoMaoPoker.CartaAlta, VencedorComparacao.Jogador1)]
    [DataRow(
        "2O 9P AE AC AP", "3O 6O 7O DO QO",
        ClassificacaoMaoPoker.Trinca, ClassificacaoMaoPoker.Flush, VencedorComparacao.Jogador2)]
    [DataRow(
        "4O 6E 9C QC QP", "3O 6O 7C QO QE",
        ClassificacaoMaoPoker.UmPar, ClassificacaoMaoPoker.UmPar, VencedorComparacao.Jogador1)]
    [DataRow(
        "2C 2O 4P 4O 4E", "3P 3O 3E 9E 9O",
        ClassificacaoMaoPoker.FullHouse, ClassificacaoMaoPoker.FullHouse, VencedorComparacao.Jogador1)]
    // O exemplo 6 repete 7O e a mão rival é Full House, não Trinca; as trocas mantêm o resultado esperado e eliminam ambos os problemas.
    [DataRow(
        "3O 4C 5E 6P 7E", "2C 3E 7P 7O 7C",
        ClassificacaoMaoPoker.Sequencia, ClassificacaoMaoPoker.Trinca, VencedorComparacao.Jogador1)]
    [DataRow(
        "9O 9C 9E 9P 2O", "2C 2E 7P 7O 7C",
        ClassificacaoMaoPoker.Quadra, ClassificacaoMaoPoker.FullHouse, VencedorComparacao.Jogador1)]
    // O exemplo 8 reserva os quatro naipes do 9 para a Quadra; 4O-8O mantém o Straight Flush sem colisão.
    [DataRow(
        "4O 5O 6O 7O 8O", "9O 9C 9E 9P 2O",
        ClassificacaoMaoPoker.StraightFlush, ClassificacaoMaoPoker.Quadra, VencedorComparacao.Jogador1)]
    [DataRow(
        "DC JC QC KC AC", "5E 6E 7E 8E 9E",
        ClassificacaoMaoPoker.RoyalFlush, ClassificacaoMaoPoker.StraightFlush, VencedorComparacao.Jogador1)]
    [DataRow(
        "4O 4C 8E 8P KO", "4E 4P 8O 8C QO",
        ClassificacaoMaoPoker.DoisPares, ClassificacaoMaoPoker.DoisPares, VencedorComparacao.Jogador1)]
    [DataRow(
        "2O 5C 8E JP AO", "2P 5E 8C JO AC",
        ClassificacaoMaoPoker.CartaAlta, ClassificacaoMaoPoker.CartaAlta, VencedorComparacao.Empate)]
    public void Comparar_ReproduzExemplosValidosDoPdf(
        string textoJogador1,
        string textoJogador2,
        ClassificacaoMaoPoker classificacaoJogador1,
        ClassificacaoMaoPoker classificacaoJogador2,
        VencedorComparacao vencedor)
    {
        var resultado = comparador.Comparar(
            MaoPoker.Parse(textoJogador1),
            MaoPoker.Parse(textoJogador2));

        Assert.AreEqual(classificacaoJogador1, resultado.ClassificacaoJogador1);
        Assert.AreEqual(classificacaoJogador2, resultado.ClassificacaoJogador2);
        Assert.AreEqual(vencedor, resultado.Vencedor);
    }

    [TestMethod]
    [DataRow("3C 4P 5E 6O 7C", "2C 3P 4E 5O 6C", VencedorComparacao.Jogador1)]
    [DataRow("2C 3P 4E 5O 6C", "2E 3O 4C 5P 6E", VencedorComparacao.Empate)]
    public void Comparar_SequenciasDesempataPelaMaiorCarta(
        string textoJogador1,
        string textoJogador2,
        VencedorComparacao vencedor)
    {
        var resultado = comparador.Comparar(
            MaoPoker.Parse(textoJogador1),
            MaoPoker.Parse(textoJogador2));

        Assert.AreEqual(vencedor, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_ParDesempataPelosKickers()
    {
        var resultado = Comparar("5C 5P KO QO 4C", "5E 5O KE JE 4P");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_ParDesempataNoSegundoKickerQuandoOPrimeiroEmpata()
    {
        var resultado = Comparar("5C 5P KC QO 4C", "5E 5O KE JE 4P");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_ParDesempataNoTerceiroKickerQuandoOsDoisPrimeirosEmpatam()
    {
        var resultado = Comparar("5C 5P KC QO 4C", "5E 5O KE QP 3C");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_DoisParesComParMaiorIgualDesempataPeloParMenor()
    {
        var resultado = Comparar("9C 9P 5E 5O KO", "9E 9O 4C 4P AC");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_DoisParesDesempataPeloParMaior()
    {
        var resultado = Comparar("9C 9P 4C 4P 2C", "8C 8P 7C 7P AO");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_DoisParesDesempataPeloKicker()
    {
        var resultado = Comparar("9C 9P 5C 5P KC", "9E 9O 5E 5O QE");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_TrincaDesempataPeloValorDaTrinca()
    {
        var resultado = Comparar("8C 8P 8E KO 4O", "7C 7P 7E AO 5O");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_FlushDesempataPelosValoresEmOrdemDecrescente()
    {
        var resultado = Comparar("AC 9C 7C 5C 2C", "AE 9E 7E 5E 3E");

        Assert.AreEqual(VencedorComparacao.Jogador2, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_FullHouseDesempataPeloValorDaTrinca()
    {
        var resultado = Comparar("8C 8P 8E 4C 4P", "7C 7P 7E AC AP");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_QuadraDesempataPeloValorDaQuadra()
    {
        var resultado = Comparar("9C 9P 9E 9O 2C", "8C 8P 8E 8O AC");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_StraightFlushDesempataPelaMaiorCarta()
    {
        var resultado = Comparar("2C 3C 4C 5C 6C", "7C 8C 9C DC JC");

        Assert.AreEqual(VencedorComparacao.Jogador2, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_RoyalFlushDeNaipesDiferentesResultaEmEmpate()
    {
        var resultado = Comparar("DO JO QO KO AO", "DC JC QC KC AC");

        Assert.AreEqual(VencedorComparacao.Empate, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_CartaAltaDesempataValoresDoMaiorParaOMenor()
    {
        var resultado = Comparar("AO KC 9E 7P 4C", "AE KE 8C 7E 4P");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_CartaAltaUsaKickerFinalQuandoOsQuatroPrimeirosEmpatam()
    {
        var resultado = Comparar("AO KC QE JP 4C", "AE KO QC JE 3P");

        Assert.AreEqual(VencedorComparacao.Jogador1, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_ValoresIguaisComNaipesDiferentesResultaEmEmpate()
    {
        var resultado = Comparar("AC KO 9E 7P 4C", "AE KC 9O 7E 4P");

        Assert.AreEqual(VencedorComparacao.Empate, resultado.Vencedor);
    }

    [TestMethod]
    [DataRow("3O 4C 5E 6P 7O", "2C 2E 7P 7O 7C")]
    [DataRow("5E 6E 7E 8E 9E", "9O 9C 9E 9P 2O")]
    public void Comparar_RejeitaCartaFisicaCompartilhada(string textoJogador1, string textoJogador2)
    {
        var excecao = Assert.ThrowsExactly<ArgumentException>(() =>
            Comparar(textoJogador1, textoJogador2));

        StringAssert.Contains(excecao.Message, "não podem compartilhar cartas físicas");
    }

    [TestMethod]
    public void Comparar_InverterJogadoresInverteVencedorEClassificacoes()
    {
        var mao1 = MaoPoker.Parse("5C 5P 6E 7P KO");
        var mao2 = MaoPoker.Parse("2P 3E 8E 8O DO");

        var resultadoOriginal = comparador.Comparar(mao1, mao2);
        var resultadoInvertido = comparador.Comparar(mao2, mao1);

        Assert.AreEqual(VencedorComparacao.Jogador2, resultadoOriginal.Vencedor);
        Assert.AreEqual(VencedorComparacao.Jogador1, resultadoInvertido.Vencedor);
        Assert.AreEqual(resultadoOriginal.ClassificacaoJogador1, resultadoInvertido.ClassificacaoJogador2);
        Assert.AreEqual(resultadoOriginal.ClassificacaoJogador2, resultadoInvertido.ClassificacaoJogador1);
    }

    [TestMethod]
    public void Comparar_AlterarOrdemDasCartasNaoAlteraResultado()
    {
        var resultadoOriginal = Comparar("5C 5P 6E 7P KO", "2P 3E 8E 8O DO");
        var resultadoReordenado = Comparar("KO 7P 6E 5P 5C", "DO 8O 8E 3E 2P");

        Assert.AreEqual(resultadoOriginal, resultadoReordenado);
    }

    [TestMethod]
    public void Comparar_SequenciaEmpataComMesmosValoresEmNaipesDiferentes()
    {
        var resultado = Comparar("2C 3P 4E 5O 6C", "2E 3O 4C 5P 6E");

        Assert.AreEqual(VencedorComparacao.Empate, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_StraightFlushEmpataComMesmosValoresEmNaipesDiferentes()
    {
        var resultado = Comparar("2C 3C 4C 5C 6C", "2P 3P 4P 5P 6P");

        Assert.AreEqual(VencedorComparacao.Empate, resultado.Vencedor);
    }

    [TestMethod]
    public void Comparar_RejeitaMaosNulas()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            comparador.Comparar(null!, MaoPoker.Parse("2C 3P 4E 5O 6C")));
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            comparador.Comparar(MaoPoker.Parse("2C 3P 4E 5O 6C"), null!));
    }

    [TestMethod]
    public void Resultado_RejeitaClassificacoesEValoresDeVencedorInvalidos()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            new ResultadoComparacaoMaoPoker(
                (ClassificacaoMaoPoker)0,
                ClassificacaoMaoPoker.CartaAlta,
                VencedorComparacao.Empate));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            new ResultadoComparacaoMaoPoker(
                ClassificacaoMaoPoker.CartaAlta,
                (ClassificacaoMaoPoker)11,
                VencedorComparacao.Empate));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            new ResultadoComparacaoMaoPoker(
                ClassificacaoMaoPoker.CartaAlta,
                ClassificacaoMaoPoker.CartaAlta,
                (VencedorComparacao)3));
    }

    [TestMethod]
    public void Resultado_PropriedadesNaoPossuemSetters()
    {
        var propriedades = typeof(ResultadoComparacaoMaoPoker).GetProperties();

        Assert.IsTrue(propriedades.All(propriedade => propriedade.SetMethod is null));
    }

    private ResultadoComparacaoMaoPoker Comparar(string textoJogador1, string textoJogador2)
    {
        return comparador.Comparar(
            MaoPoker.Parse(textoJogador1),
            MaoPoker.Parse(textoJogador2));
    }
}
