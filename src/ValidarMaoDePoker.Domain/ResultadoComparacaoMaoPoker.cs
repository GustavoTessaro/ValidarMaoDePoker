namespace ValidarMaoDePoker.Domain;

public sealed record ResultadoComparacaoMaoPoker
{
    public ResultadoComparacaoMaoPoker(
        ClassificacaoMaoPoker classificacaoJogador1,
        ClassificacaoMaoPoker classificacaoJogador2,
        VencedorComparacao vencedor)
    {
        if (!Enum.IsDefined(classificacaoJogador1))
        {
            throw new ArgumentOutOfRangeException(nameof(classificacaoJogador1), "A classificação do Jogador 1 é inválida.");
        }

        if (!Enum.IsDefined(classificacaoJogador2))
        {
            throw new ArgumentOutOfRangeException(nameof(classificacaoJogador2), "A classificação do Jogador 2 é inválida.");
        }

        if (!Enum.IsDefined(vencedor))
        {
            throw new ArgumentOutOfRangeException(nameof(vencedor), "O vencedor da comparação é inválido.");
        }

        ClassificacaoJogador1 = classificacaoJogador1;
        ClassificacaoJogador2 = classificacaoJogador2;
        Vencedor = vencedor;
    }

    public ClassificacaoMaoPoker ClassificacaoJogador1 { get; }

    public ClassificacaoMaoPoker ClassificacaoJogador2 { get; }

    public VencedorComparacao Vencedor { get; }
}
