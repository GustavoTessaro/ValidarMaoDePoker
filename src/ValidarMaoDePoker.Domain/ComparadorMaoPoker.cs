namespace ValidarMaoDePoker.Domain;

public sealed class ComparadorMaoPoker
{
    private readonly ClassificadorMaoPoker classificador = new();

    public ResultadoComparacaoMaoPoker Comparar(MaoPoker maoJogador1, MaoPoker maoJogador2)
    {
        ArgumentNullException.ThrowIfNull(maoJogador1);
        ArgumentNullException.ThrowIfNull(maoJogador2);

        if (maoJogador1.Cartas.Intersect(maoJogador2.Cartas).Any())
        {
            throw new ArgumentException("Os jogadores não podem compartilhar cartas físicas.");
        }

        var classificacaoJogador1 = classificador.Classificar(maoJogador1);
        var classificacaoJogador2 = classificador.Classificar(maoJogador2);
        var comparacao = classificacaoJogador1.CompareTo(classificacaoJogador2);

        if (comparacao == 0)
        {
            comparacao = CompararDesempate(maoJogador1, maoJogador2, classificacaoJogador1);
        }

        var vencedor = comparacao switch
        {
            > 0 => VencedorComparacao.Jogador1,
            < 0 => VencedorComparacao.Jogador2,
            _ => VencedorComparacao.Empate
        };

        return new ResultadoComparacaoMaoPoker(
            classificacaoJogador1,
            classificacaoJogador2,
            vencedor);
    }

    private static int CompararDesempate(
        MaoPoker maoJogador1,
        MaoPoker maoJogador2,
        ClassificacaoMaoPoker classificacao)
    {
        if (classificacao == ClassificacaoMaoPoker.RoyalFlush)
        {
            return 0;
        }

        var chaveJogador1 = ObterChaveDesempate(maoJogador1, classificacao);
        var chaveJogador2 = ObterChaveDesempate(maoJogador2, classificacao);

        for (var indice = 0; indice < chaveJogador1.Length; indice++)
        {
            var comparacao = chaveJogador1[indice].CompareTo(chaveJogador2[indice]);
            if (comparacao != 0)
            {
                return comparacao;
            }
        }

        return 0;
    }

    private static int[] ObterChaveDesempate(MaoPoker mao, ClassificacaoMaoPoker classificacao)
    {
        var valores = mao.Cartas
            .Select(carta => (int)carta.Valor)
            .OrderDescending()
            .ToArray();

        if (classificacao is ClassificacaoMaoPoker.Sequencia or ClassificacaoMaoPoker.StraightFlush)
        {
            return [valores[0]];
        }

        if (classificacao is ClassificacaoMaoPoker.CartaAlta or ClassificacaoMaoPoker.Flush)
        {
            return valores;
        }

        return mao.Cartas
            .GroupBy(carta => carta.Valor)
            .OrderByDescending(grupo => grupo.Count())
            .ThenByDescending(grupo => grupo.Key)
            .SelectMany(grupo => Enumerable.Repeat((int)grupo.Key, grupo.Count()))
            .ToArray();
    }
}
