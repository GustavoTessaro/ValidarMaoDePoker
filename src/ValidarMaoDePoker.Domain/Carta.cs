namespace ValidarMaoDePoker.Domain;

public readonly record struct Carta
{
    public Carta(ValorCarta valor, Naipe naipe)
    {
        if (!Enum.IsDefined(valor))
        {
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor da carta é inválido.");
        }

        if (!Enum.IsDefined(naipe))
        {
            throw new ArgumentOutOfRangeException(nameof(naipe), "O naipe da carta é inválido.");
        }

        Valor = valor;
        Naipe = naipe;
    }

    public ValorCarta Valor { get; }

    public Naipe Naipe { get; }

    public static Carta Parse(string? texto)
    {
        ArgumentNullException.ThrowIfNull(texto);

        if (texto.Length != 2)
        {
            throw new FormatException("Uma carta deve conter exatamente dois caracteres.");
        }

        var valor = texto[0] switch
        {
            >= '2' and <= '9' => (ValorCarta)(texto[0] - '0'),
            'D' => ValorCarta.Dez,
            'J' => ValorCarta.Valete,
            'Q' => ValorCarta.Dama,
            'K' => ValorCarta.Rei,
            'A' => ValorCarta.As,
            _ => throw new FormatException($"Valor de carta inválido: '{texto[0]}'.")
        };

        var naipe = texto[1] switch
        {
            'O' => Naipe.Ouros,
            'C' => Naipe.Copas,
            'E' => Naipe.Espadas,
            'P' => Naipe.Paus,
            _ => throw new FormatException($"Naipe inválido: '{texto[1]}'.")
        };

        return new Carta(valor, naipe);
    }

    public override string ToString()
    {
        var valor = Valor switch
        {
            >= ValorCarta.Dois and <= ValorCarta.Nove => ((int)Valor).ToString(),
            ValorCarta.Dez => "D",
            ValorCarta.Valete => "J",
            ValorCarta.Dama => "Q",
            ValorCarta.Rei => "K",
            ValorCarta.As => "A",
            _ => throw new InvalidOperationException("O valor da carta é inválido.")
        };

        var naipe = Naipe switch
        {
            Naipe.Ouros => "O",
            Naipe.Copas => "C",
            Naipe.Espadas => "E",
            Naipe.Paus => "P",
            _ => throw new InvalidOperationException("O naipe da carta é inválido.")
        };

        return valor + naipe;
    }
}
