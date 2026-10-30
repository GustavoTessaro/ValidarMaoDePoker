using System.Collections.ObjectModel;

namespace ValidarMaoDePoker.Domain;

public sealed class MaoPoker
{
    public const int QuantidadeCartas = 5;

    private readonly ReadOnlyCollection<Carta> cartas;

    public MaoPoker(IEnumerable<Carta> cartas)
    {
        ArgumentNullException.ThrowIfNull(cartas);

        var copia = cartas.ToArray();
        if (copia.Length != QuantidadeCartas)
        {
            throw new ArgumentException("Uma mão de Poker deve conter exatamente cinco cartas.", nameof(cartas));
        }

        if (copia.Any(carta => carta is null))
        {
            throw new ArgumentException("Uma mão não pode conter cartas nulas.", nameof(cartas));
        }

        if (copia.Distinct().Count() != QuantidadeCartas)
        {
            throw new ArgumentException("Uma mão não pode conter cartas repetidas.", nameof(cartas));
        }

        this.cartas = Array.AsReadOnly(copia);
    }

    public IReadOnlyList<Carta> Cartas => cartas;

    public static MaoPoker Parse(string? texto)
    {
        ArgumentNullException.ThrowIfNull(texto);

        if (string.IsNullOrWhiteSpace(texto))
        {
            throw new ArgumentException("A mão não pode ser vazia.", nameof(texto));
        }

        var cartas = texto.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(Carta.Parse);

        return new MaoPoker(cartas);
    }
}
