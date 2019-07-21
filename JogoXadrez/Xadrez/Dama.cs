﻿using Estrutura;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xadrez
{
    public class Dama : Peca
    {
        public Dama(Cor cor, Tabuleiro tabuleiro) : base(cor, tabuleiro)
        {
        }

        public override string ToString()
        {
            return "D";
        }

        private bool PodeMover(Posicao posicao)
        {
            Peca peca = Tabuleiro.ObterPeca(posicao);
            return peca == null || peca.Cor != this.Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] movimentos = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            Posicao aux = new Posicao(0, 0);

            aux.DefinirPosicao(Posicao.Linha - 1, Posicao.Coluna);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Linha -= 1;
            }

            aux.DefinirPosicao(Posicao.Linha + 1, Posicao.Coluna);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Linha += 1;
            }

            aux.DefinirPosicao(Posicao.Linha, Posicao.Coluna + 1);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Coluna += 1;
            }

            aux.DefinirPosicao(Posicao.Linha, Posicao.Coluna - 1);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Coluna -= 1;
            }

            aux.DefinirPosicao(Posicao.Linha - 1, Posicao.Coluna - 1);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Linha -= 1;
                aux.Coluna -= 1;
            }

            aux.DefinirPosicao(Posicao.Linha + 1, Posicao.Coluna + 1);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Linha += 1;
                aux.Coluna += 1;
            }

            aux.DefinirPosicao(Posicao.Linha - 1, Posicao.Coluna + 1);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Coluna += 1;
                aux.Linha -= 1;
            }

            aux.DefinirPosicao(Posicao.Linha + 1, Posicao.Coluna - 1);
            while (Tabuleiro.VerificarPosicao(aux) && PodeMover(aux))
            {
                movimentos[aux.Linha, aux.Coluna] = true;
                if (Tabuleiro.ObterPeca(aux) != null && Tabuleiro.ObterPeca(aux).Cor != this.Cor)
                    break;
                aux.Coluna -= 1;
                aux.Linha += 1;
            }

            return movimentos;
        }
    }
}
