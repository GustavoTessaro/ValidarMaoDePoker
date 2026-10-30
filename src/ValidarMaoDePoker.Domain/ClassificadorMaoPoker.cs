namespace ValidarMaoDePoker.Domain;

public sealed class ClassificadorMaoPoker
{
    public ClassificacaoMaoPoker Classificar(MaoPoker mao)
    {
        ArgumentNullException.ThrowIfNull(mao);

        var cartas = mao.Cartas;
        var valores = cartas.Select(carta => (int)carta.Valor).Order().ToArray();
        var frequencias = cartas
            .GroupBy(carta => carta.Valor)
            .Select(grupo => grupo.Count())
            .OrderDescending()
            .ToArray();
        var ehFlush = cartas.All(carta => carta.Naipe == cartas[0].Naipe);
        var ehSequencia = valores.Distinct().Count() == MaoPoker.QuantidadeCartas
            && valores[^1] - valores[0] == MaoPoker.QuantidadeCartas - 1;

        if (ehFlush && ehSequencia)
        {
            return valores[0] == (int)ValorCarta.Dez
                ? ClassificacaoMaoPoker.RoyalFlush
                : ClassificacaoMaoPoker.StraightFlush;
        }

        if (frequencias[0] == 4)
        {
            return ClassificacaoMaoPoker.Quadra;
        }

        if (frequencias[0] == 3 && frequencias[1] == 2)
        {
            return ClassificacaoMaoPoker.FullHouse;
        }

        if (ehFlush)
        {
            return ClassificacaoMaoPoker.Flush;
        }

        if (ehSequencia)
        {
            return ClassificacaoMaoPoker.Sequencia;
        }

        if (frequencias[0] == 3)
        {
            return ClassificacaoMaoPoker.Trinca;
        }

        if (frequencias[0] == 2 && frequencias[1] == 2)
        {
            return ClassificacaoMaoPoker.DoisPares;
        }

        if (frequencias[0] == 2)
        {
            return ClassificacaoMaoPoker.UmPar;
        }

        return ClassificacaoMaoPoker.CartaAlta;
    }
}
