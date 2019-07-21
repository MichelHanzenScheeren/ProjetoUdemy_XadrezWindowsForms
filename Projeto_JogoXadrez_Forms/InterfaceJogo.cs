using Estrutura;
using Excessoes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xadrez;

namespace Projeto_JogoXadrez_Forms
{
    public partial class InterfaceJogo : Form
    {
        public PartidaDeXadrez PartidaDeXadrez { get; set; }
        public Posicao Origem { get; set; }
        public InterfaceJogo()
        {
            InitializeComponent();
            IniciarPartida();
        }

        public void IniciarPartida()
        {
            PartidaDeXadrez = new PartidaDeXadrez();
            Origem = null;
            AtualizarDataGrid();
        }

        public void AtualizarDataGrid()
        {
            AjustarLegenda();
            for (int i = 0; i < PartidaDeXadrez.Tabuleiro.Linhas; i++)
            {
                for (int j = 0; j < PartidaDeXadrez.Tabuleiro.Colunas; j++)
                {
                    FormatarFundoDataGrid(i, j);
                    Peca peca = PartidaDeXadrez.Tabuleiro.ObterPeca(i, j);
                    if(peca != null)
                    {
                        if (peca.Cor == Cor.Azul)
                            dgvXadrez.Rows[i].Cells[j].Style.ForeColor = Color.DarkBlue;
                        else
                            dgvXadrez.Rows[i].Cells[j].Style.ForeColor = Color.Red;
                        
                        dgvXadrez.Rows[i].Cells[j].Value = peca;
                    }
                }
            }
            dgvXadrez.ClearSelection();
            AtualizarInformacoes();
        }

        private void AtualizarInformacoes()
        {
            lblTurno.Text = PartidaDeXadrez.Turno + "º";
            lblJogador.Text = PartidaDeXadrez.JogadorAtual.ToString().ToUpper();
            if (lblJogador.Text == "AZUL")
                lblJogador.ForeColor = Color.DarkBlue;
            else
                lblJogador.ForeColor = Color.Red;

            lblCapturadasAzul.Text = "[  ";
            lblCapturadasVermelho.Text = "[  ";
            foreach (var item in PartidaDeXadrez.PecasCapturadas(Cor.Azul))
            {
                lblCapturadasAzul.Text += item + "  ";
            }
            foreach (var item in PartidaDeXadrez.PecasCapturadas(Cor.Vermelho))
            {
                lblCapturadasVermelho.Text += item + "  ";
            }
            lblCapturadasAzul.Text += "]";
            lblCapturadasVermelho.Text += "]";

            TimeSpan aux = PartidaDeXadrez.TempoJogadorA;
            lblTempoA.Text = PartidaDeXadrez.AjustarTempo(aux);
            aux = PartidaDeXadrez.TempoJogadorB;
            lblTempoB.Text = PartidaDeXadrez.AjustarTempo(aux);
            aux += PartidaDeXadrez.TempoJogadorA;
            lblTempoTotal.Text = PartidaDeXadrez.AjustarTempo(aux);
        }

        private void AjustarLegenda()
        {
            dgvXadrez.Rows.Clear();
            int contLinha = 0;
            for (int i = 0; i < 8; i++)
            {
                contLinha = dgvXadrez.Rows.Add();
                dgvXadrez.Rows[contLinha].Cells[8].Value = i + 1;
            }
            char coluna = 'A';
            contLinha = dgvXadrez.Rows.Add();
            for (int i = 0; i < 8; i++)
            {
                dgvXadrez.Rows[contLinha].Cells[i].Value = coluna;
                coluna = Convert.ToChar(coluna + 1);
            }
            dgvXadrez.Rows[contLinha].Height = 30;
        }

        public void FormatarFundoDataGrid(int linha, int coluna)
        {
            Color corA = Color.Black;
            Color corB = Color.White;
            if (linha % 2 == 0)
            {
                if (coluna % 2 == 0)
                    dgvXadrez.Rows[linha].Cells[coluna].Style.BackColor = corA;
                else
                    dgvXadrez.Rows[linha].Cells[coluna].Style.BackColor = corB;
            }
            else
            {
                if (coluna % 2 == 0)
                    dgvXadrez.Rows[linha].Cells[coluna].Style.BackColor = corB;
                else
                    dgvXadrez.Rows[linha].Cells[coluna].Style.BackColor = corA;
            }
        }

        private void DgvXadrez_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int linha = dgvXadrez.SelectedCells[0].RowIndex;
                int coluna = dgvXadrez.SelectedCells[0].ColumnIndex;
                Posicao posicao = new Posicao(dgvXadrez.SelectedCells[0].RowIndex, dgvXadrez.SelectedCells[0].ColumnIndex);
                if (Origem == null)
                {
                    PartidaDeXadrez.ValidarOrigem(posicao);
                    Origem = posicao;
                    AjustarOrigem(linha, coluna);
                }
                else
                {
                    Posicao origem = Origem;
                    Origem = null;
                    PartidaDeXadrez.ValidarDestino(origem, posicao);
                    PartidaDeXadrez.NovaJogada(origem, posicao);
                    TestarPromocao(PartidaDeXadrez.Tabuleiro.ObterPeca(posicao));
                    AtualizarDataGrid();
                    TesteXeque();
                }
            }
            catch (TabuleiroException erro)
            {
                MessageBox.Show(erro.Message);
                AtualizarDataGrid();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("A POSIÇÃO INFORMADA É INVÁLIDA!");
                AtualizarDataGrid();
            }
            catch (XequeMateException)
            {
                MessageBox.Show("É XEQUE-MATE!\nVENCEDOR: "+ PartidaDeXadrez.JogadorAdversario());
                Close();
            }
        }

        private void AjustarOrigem(int linha, int coluna)
        {
            AtualizarDataGrid();
            Peca peca = PartidaDeXadrez.Tabuleiro.ObterPeca(linha, coluna);
            ExibirJogadasPossiveis(peca.MovimentosPossiveis());
            dgvXadrez.Rows[linha].Cells[coluna].Style.BackColor = Color.Gray;

            if (peca.Cor == Cor.Azul)
                dgvXadrez.Rows[linha].Cells[coluna].Style.ForeColor = Color.DarkBlue;
            else
                dgvXadrez.Rows[linha].Cells[coluna].Style.ForeColor = Color.DarkRed;
        }

        private void ExibirJogadasPossiveis(bool[,] movimentosPossiveis)
        {
            for (int i = 0; i < PartidaDeXadrez.Tabuleiro.Linhas; i++)
            {
                for (int j = 0; j < PartidaDeXadrez.Tabuleiro.Colunas; j++)
                {
                    if (movimentosPossiveis[i, j])
                        dgvXadrez.Rows[i].Cells[j].Style.BackColor = Color.Silver;
                }
            }
        }

        private void TesteXeque()
        {
            if (PartidaDeXadrez.EstaEmXeque())
                MessageBox.Show("   XEQUE!");
        }

        private void TestarPromocao(Peca peca)
        {
            //Teste Jogada Especial "Promoção"(trocar peão por dama se chegar ao outro lado do tabuleiro)
            if (peca is Peao)
            {
                if ((peca.Cor == Cor.Azul && peca.Posicao.Linha == 7) || (peca.Cor == Cor.Vermelho && peca.Posicao.Linha == 0))
                {
                    PartidaDeXadrez.Promocao(peca);
                    MessageBox.Show("JOGADA ESPECIAL PROMOÇÃO DETECTADA!\n\nPELAS REGRAS DO XADREZ:\n* SE UM PEÃO ATINGE O FIM DO TABULEIRO ADVERSÁRIO, É SUSBTITUÍDO POR UMA DAMA!");
                }
            }
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

    }
}
