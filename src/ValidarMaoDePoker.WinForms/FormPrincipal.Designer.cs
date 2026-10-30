namespace ValidarMaoDePoker.WinForms;

partial class FormPrincipal
{
    private static readonly Color Fundo = Color.FromArgb(18, 24, 22);
    private static readonly Color FundoCartao = Color.FromArgb(29, 38, 35);
    private static readonly Color FundoCampo = Color.FromArgb(14, 19, 17);
    private static readonly Color FundoCampoErro = Color.FromArgb(67, 35, 35);
    private static readonly Color Verde = Color.FromArgb(52, 211, 153);
    private static readonly Color TextoPrincipal = Color.FromArgb(240, 245, 242);
    private static readonly Color TextoSecundario = Color.FromArgb(163, 177, 170);
    private static readonly Color CorErro = Color.FromArgb(255, 139, 139);

    private TextBox campoCartasJogador1 = null!;
    private TextBox campoCartasJogador2 = null!;
    private Label classificacaoJogador1 = null!;
    private Label classificacaoJogador2 = null!;
    private Label valorResultado = null!;
    private Label mensagemResultado = null!;
    private Button botaoComparar = null!;
    private Button botaoLimpar = null!;

    private void InitializeComponent()
    {
        SuspendLayout();

        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Fundo;
        ClientSize = new Size(1080, 760);
        MinimumSize = new Size(800, 680);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Validador de Mão de Poker";
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        ForeColor = TextoPrincipal;

        var layoutPrincipal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Fundo,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(34, 28, 34, 20)
        };
        layoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var cabecalho = CriarCabecalho();
        var painelJogadores = CriarPainelJogadores();
        var painelAcoes = CriarPainelAcoes();
        var painelResultado = CriarPainelResultado();
        var rodape = CriarRodape();

        layoutPrincipal.Controls.Add(cabecalho, 0, 0);
        layoutPrincipal.Controls.Add(painelJogadores, 0, 1);
        layoutPrincipal.Controls.Add(painelAcoes, 0, 2);
        layoutPrincipal.Controls.Add(painelResultado, 0, 3);
        layoutPrincipal.Controls.Add(rodape, 0, 4);

        Controls.Add(layoutPrincipal);
        ResumeLayout(performLayout: true);
    }

    private Control CriarCabecalho()
    {
        var painel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 3,
            Margin = new Padding(0, 0, 0, 26)
        };
        painel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        var indicador = CriarLabel("VALIDAÇÃO DE PÔQUER", 9F, Verde, FontStyle.Bold);
        indicador.Margin = new Padding(0, 0, 0, 6);

        var titulo = CriarLabel("Validador de Mão de Poker", 25F, TextoPrincipal, FontStyle.Bold);
        titulo.Margin = new Padding(0, 0, 0, 4);

        var subtitulo = CriarLabel(
            "Compare duas mãos de cinco cartas e descubra qual tem a melhor classificação.",
            10.5F,
            TextoSecundario);

        painel.Controls.Add(indicador, 0, 0);
        painel.Controls.Add(titulo, 0, 1);
        painel.Controls.Add(subtitulo, 0, 2);
        return painel;
    }

    private Control CriarPainelJogadores()
    {
        var painel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 20)
        };
        painel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        painel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        painel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var cartaoJogador1 = CriarCartaoJogador(
            "JOGADOR 1",
            "Informe as cartas da primeira mão.",
            out campoCartasJogador1,
            out classificacaoJogador1);
        cartaoJogador1.Margin = new Padding(0, 0, 10, 0);

        var cartaoJogador2 = CriarCartaoJogador(
            "JOGADOR 2",
            "Informe as cartas da segunda mão.",
            out campoCartasJogador2,
            out classificacaoJogador2);
        cartaoJogador2.Margin = new Padding(10, 0, 0, 0);

        painel.Controls.Add(cartaoJogador1, 0, 0);
        painel.Controls.Add(cartaoJogador2, 1, 0);
        return painel;
    }

    private Control CriarCartaoJogador(
        string jogador,
        string descricao,
        out TextBox campoCartas,
        out Label classificacao)
    {
        var cartao = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = FundoCartao,
            ColumnCount = 1,
            RowCount = 7,
            Padding = new Padding(22),
            Margin = Padding.Empty
        };
        cartao.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        cartao.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        cartao.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        cartao.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        cartao.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        cartao.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        cartao.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        cartao.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var titulo = CriarLabel(jogador, 11F, Verde, FontStyle.Bold);
        titulo.Margin = new Padding(0, 0, 0, 5);

        var textoDescricao = CriarLabel(descricao, 9.5F, TextoSecundario);
        textoDescricao.Margin = new Padding(0, 0, 0, 20);

        var rotuloCampo = CriarLabel("Cartas da mão", 9F, TextoPrincipal, FontStyle.Bold);
        rotuloCampo.Margin = new Padding(0, 0, 0, 7);

        campoCartas = new TextBox
        {
            Dock = DockStyle.Top,
            AutoSize = false,
            Height = 48,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = FundoCampo,
            ForeColor = TextoPrincipal,
            Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point),
            PlaceholderText = "5C 5P 6E 7P KO",
            MaxLength = 14,
            Margin = new Padding(0, 0, 0, 8)
        };
        campoCartas.AccessibleName = $"Cartas de {jogador.ToLowerInvariant()}";

        var dica = CriarLabel("Cinco cartas separadas por espaços.", 8.5F, TextoSecundario);
        dica.Margin = new Padding(0, 0, 0, 22);

        var painelClassificacao = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = FundoCartao,
            Margin = Padding.Empty,
            Padding = new Padding(0, 14, 0, 0)
        };
        painelClassificacao.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        painelClassificacao.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        painelClassificacao.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var tituloClassificacao = CriarLabel("CLASSIFICAÇÃO", 8F, TextoSecundario, FontStyle.Bold);
        tituloClassificacao.Margin = new Padding(0, 0, 0, 5);
        classificacao = CriarLabel("Aguardando comparação", 10F, TextoSecundario);
        painelClassificacao.Controls.Add(tituloClassificacao, 0, 0);
        painelClassificacao.Controls.Add(classificacao, 0, 1);

        cartao.Controls.Add(titulo, 0, 0);
        cartao.Controls.Add(textoDescricao, 0, 1);
        cartao.Controls.Add(rotuloCampo, 0, 2);
        cartao.Controls.Add(campoCartas, 0, 3);
        cartao.Controls.Add(dica, 0, 4);
        cartao.Controls.Add(painelClassificacao, 0, 6);
        return cartao;
    }

    private Control CriarPainelAcoes()
    {
        var painel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 18),
            Padding = Padding.Empty
        };

        botaoComparar = new Button
        {
            Text = "Comparar mãos",
            Enabled = true,
            Width = 172,
            Height = 44,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(36, 90, 68),
            ForeColor = Color.FromArgb(190, 211, 201),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point),
            Cursor = Cursors.Default,
            Margin = new Padding(10, 0, 0, 0)
        };
        botaoComparar.FlatAppearance.BorderSize = 0;
        botaoComparar.AccessibleDescription = "Será habilitado após a integração com o domínio.";

        botaoLimpar = new Button
        {
            Text = "Limpar",
            Width = 112,
            Height = 44,
            FlatStyle = FlatStyle.Flat,
            BackColor = FundoCartao,
            ForeColor = TextoPrincipal,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
            Cursor = Cursors.Hand,
            Margin = new Padding(0)
        };
        botaoLimpar.FlatAppearance.BorderColor = Color.FromArgb(67, 82, 75);
        botaoLimpar.FlatAppearance.BorderSize = 1;
        botaoLimpar.AccessibleName = "Limpar campos";

        painel.Controls.Add(botaoComparar);
        painel.Controls.Add(botaoLimpar);
        return painel;
    }

    private Control CriarPainelResultado()
    {
        var painel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            BackColor = Color.FromArgb(23, 45, 36),
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(18, 14, 18, 14),
            Margin = new Padding(0, 0, 0, 18)
        };
        painel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        painel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        painel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        painel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var titulo = CriarLabel("RESULTADO", 8F, Verde, FontStyle.Bold);
        titulo.Margin = new Padding(0, 0, 0, 4);
        valorResultado = CriarLabel("Resultado ainda não disponível", 13F, TextoPrincipal, FontStyle.Bold);
        valorResultado.Margin = new Padding(0, 0, 0, 4);
        var mensagem = CriarLabel(
            "Informe as duas mãos e selecione Comparar mãos.",
            10F,
            TextoSecundario);

        painel.Controls.Add(titulo, 0, 0);
        painel.Controls.Add(valorResultado, 0, 1);
        painel.Controls.Add(mensagem, 0, 2);
        mensagemResultado = mensagem;
        return painel;
    }

    private Control CriarRodape()
    {
        var rodape = CriarLabel("VALIDADOR DE MÃO DE POKER  •  APLICAÇÃO LOCAL", 8F, TextoSecundario);
        rodape.Dock = DockStyle.Top;
        rodape.TextAlign = ContentAlignment.MiddleCenter;
        rodape.Margin = new Padding(0, 0, 0, 0);
        return rodape;
    }

    private static Label CriarLabel(
        string texto,
        float tamanhoFonte,
        Color cor,
        FontStyle estilo = FontStyle.Regular)
    {
        return new Label
        {
            AutoSize = true,
            Text = texto,
            ForeColor = cor,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", tamanhoFonte, estilo, GraphicsUnit.Point),
            Margin = Padding.Empty
        };
    }
}
