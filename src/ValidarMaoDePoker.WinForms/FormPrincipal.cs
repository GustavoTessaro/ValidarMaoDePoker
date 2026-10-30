using ValidarMaoDePoker.Domain;

namespace ValidarMaoDePoker.WinForms;

public partial class FormPrincipal : Form
{
    private readonly ComparadorMaoPoker comparador = new();

    public FormPrincipal()
    {
        InitializeComponent();
        botaoComparar.Click += BotaoComparar_Click;
        botaoLimpar.Click += BotaoLimpar_Click;
        campoCartasJogador1.KeyDown += CampoCartas_KeyDown;
        campoCartasJogador2.KeyDown += CampoCartas_KeyDown;
        campoCartasJogador1.TextChanged += CampoCartas_TextChanged;
        campoCartasJogador2.TextChanged += CampoCartas_TextChanged;
    }

    private void BotaoComparar_Click(object? sender, EventArgs e)
    {
        LimparFeedback();

        MaoPoker maoJogador1;
        try
        {
            maoJogador1 = MaoPoker.Parse(campoCartasJogador1.Text);
        }
        catch (FormatException excecao)
        {
            ExibirErro(campoCartasJogador1, excecao.Message);
            return;
        }
        catch (ArgumentException excecao)
        {
            ExibirErro(campoCartasJogador1, excecao.Message);
            return;
        }

        MaoPoker maoJogador2;
        try
        {
            maoJogador2 = MaoPoker.Parse(campoCartasJogador2.Text);
        }
        catch (FormatException excecao)
        {
            ExibirErro(campoCartasJogador2, excecao.Message);
            return;
        }
        catch (ArgumentException excecao)
        {
            ExibirErro(campoCartasJogador2, excecao.Message);
            return;
        }

        ResultadoComparacaoMaoPoker resultado;
        try
        {
            resultado = comparador.Comparar(maoJogador1, maoJogador2);
        }
        catch (ArgumentException excecao)
        {
            campoCartasJogador1.BackColor = FundoCampoErro;
            campoCartasJogador2.BackColor = FundoCampoErro;
            ExibirMensagemErro(excecao.Message);
            campoCartasJogador1.Focus();
            return;
        }

        classificacaoJogador1.Text = resultado.ClassificacaoJogador1.NomeEmPortugues();
        classificacaoJogador2.Text = resultado.ClassificacaoJogador2.NomeEmPortugues();
        (valorResultado.Text, mensagemResultado.Text) = resultado.Vencedor switch
        {
            VencedorComparacao.Jogador1 => (
                "Jogador 1 venceu",
                "A mão do Jogador 1 tem a melhor classificação."),
            VencedorComparacao.Jogador2 => (
                "Jogador 2 venceu",
                "A mão do Jogador 2 tem a melhor classificação."),
            VencedorComparacao.Empate => (
                "Empate",
                "As mãos têm o mesmo valor segundo as regras de comparação."),
            _ => throw new InvalidOperationException("O domínio retornou um vencedor inválido.")
        };
        mensagemResultado.ForeColor = TextoSecundario;
    }

    private void BotaoLimpar_Click(object? sender, EventArgs e)
    {
        campoCartasJogador1.Clear();
        campoCartasJogador2.Clear();
        RestaurarVisualCampo(campoCartasJogador1);
        RestaurarVisualCampo(campoCartasJogador2);
        LimparFeedback();
        campoCartasJogador1.Focus();
    }

    private void CampoCartas_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
        {
            return;
        }

        e.SuppressKeyPress = true;
        BotaoComparar_Click(sender, EventArgs.Empty);
    }

    private void CampoCartas_TextChanged(object? sender, EventArgs e)
    {
        if (sender is TextBox campo)
        {
            RestaurarVisualCampo(campo);
        }
    }

    private void LimparFeedback()
    {
        classificacaoJogador1.Text = "Aguardando comparação";
        classificacaoJogador2.Text = "Aguardando comparação";
        valorResultado.Text = "Resultado ainda não disponível";
        mensagemResultado.Text = "Informe as duas mãos e selecione Comparar mãos.";
        mensagemResultado.ForeColor = TextoSecundario;
        RestaurarVisualCampo(campoCartasJogador1);
        RestaurarVisualCampo(campoCartasJogador2);
    }

    private void ExibirErro(TextBox campo, string mensagem)
    {
        campo.BackColor = FundoCampoErro;
        ExibirMensagemErro(mensagem);
        campo.Focus();
    }

    private void ExibirMensagemErro(string mensagem)
    {
        mensagemResultado.Text = mensagem;
        mensagemResultado.ForeColor = CorErro;
    }

    private static void RestaurarVisualCampo(TextBox campo)
    {
        campo.BackColor = FundoCampo;
    }
}
