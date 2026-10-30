namespace ValidarMaoDePoker.Domain;

public static class ClassificacaoMaoPokerExtensions
{
    public static string NomeEmPortugues(this ClassificacaoMaoPoker classificacao)
    {
        return classificacao switch
        {
            ClassificacaoMaoPoker.CartaAlta => "Carta Alta",
            ClassificacaoMaoPoker.UmPar => "Um Par",
            ClassificacaoMaoPoker.DoisPares => "Dois Pares",
            ClassificacaoMaoPoker.Trinca => "Trinca",
            ClassificacaoMaoPoker.Sequencia => "Sequência",
            ClassificacaoMaoPoker.Flush => "Flush",
            ClassificacaoMaoPoker.FullHouse => "Full House",
            ClassificacaoMaoPoker.Quadra => "Quadra",
            ClassificacaoMaoPoker.StraightFlush => "Straight Flush",
            ClassificacaoMaoPoker.RoyalFlush => "Royal Flush",
            _ => throw new ArgumentOutOfRangeException(nameof(classificacao), classificacao, "Classificação de mão inválida.")
        };
    }
}
