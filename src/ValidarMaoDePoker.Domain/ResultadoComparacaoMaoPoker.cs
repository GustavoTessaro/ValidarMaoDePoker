namespace ValidarMaoDePoker.Domain;

public sealed record ResultadoComparacaoMaoPoker
{
    public ResultadoComparacaoMaoPoker(
        ClassificacaoMaoPoker classificacaoJogador1,
        ClassificacaoMaoPoker classificacaoJogador2,
        VencedorComparacao vencedor)
    {
        ClassificacaoJogador1 = classificacaoJogador1;
        ClassificacaoJogador2 = classificacaoJogador2;
        Vencedor = vencedor;
    }

    public ClassificacaoMaoPoker ClassificacaoJogador1 { get; }

    public ClassificacaoMaoPoker ClassificacaoJogador2 { get; }

    public VencedorComparacao Vencedor { get; }
}
