namespace ValidarMaoDePoker.WinForms;

public partial class FormPrincipal : Form
{
    public FormPrincipal()
    {
        InitializeComponent();
        botaoLimpar.Click += BotaoLimpar_Click;
    }

    private void BotaoLimpar_Click(object? sender, EventArgs e)
    {
        campoCartasJogador1.Clear();
        campoCartasJogador2.Clear();
        campoCartasJogador1.Focus();
    }
}
