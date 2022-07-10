﻿using Moq;
using Xunit;
using System;
using Aliquota.Domain.Interfaces;
using Aliquota.Domain.Models;
using Aliquota.Domain.Services;
using Moq.AutoMock;

namespace Aliquota.Domain.Tests
{
    public class PosicaoTests
    {
        [Fact(DisplayName = "01 Adicionar Posição Válida")]
        [Trait("Categoria", "Posicao")]
        public async void AdicionarPosicao_NovoPosicaoValida_DeveCadastrarPosicao()
        {
            // AAA
            var posicao = new Posicao(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now, 100.14m, true);

            var mocker = new AutoMocker();
            var posicaoService = mocker.CreateInstance<PosicaoService>(); 

            /*
            var posicaoRepo = new Mock<IPosicaoRepository>();
            var produtoRepo = new Mock<IProdutoRepository>();
            var notificador = new Mock<INotificador>();
            var posicaoService = new PosicaoService(posicaoRepo.Object, produtoRepo.Object, notificador.Object); */

            // ACT
            await posicaoService.Adicionar(posicao);

            // Assert
            mocker.GetMock<IPosicaoRepository>().Verify(r => r.Adicionar(posicao), Times.Once);
        }

        [Fact(DisplayName = "02 Adicionar Posição Invalida")]
        [Trait("Categoria", "Posicao")]
        public async void AdicionarPosicao_NovoPosicaoInalida_NaoDeveCadastrarPosicao()
        {
            // AAA
            var posicao = new Posicao(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now, 0, true);

            var mocker = new AutoMocker();
            var posicaoService = mocker.CreateInstance<PosicaoService>();

            // ACT
            await posicaoService.Adicionar(posicao);

            // Assert
            mocker.GetMock<IPosicaoRepository>().Verify(r => r.Adicionar(posicao), Times.Never);
        }


        [Fact(DisplayName = "03 Atualizar Posição Válida")]
        [Trait("Categoria", "Posicao")]
        public async void AtualizarPosicao_AtualizarPosicaoValida_DeveAtualizarPosicao()
        {
            // AAA
            var produtoId = Guid.NewGuid();
            var posicaoId = Guid.NewGuid();

            var posicao = new Posicao(produtoId, posicaoId, DateTime.Now, DateTime.Now, 100, true);

            var mocker = new AutoMocker();
            var posicaoService = mocker.CreateInstance<PosicaoService>();

            await posicaoService.Adicionar(posicao);

            var posicaoAtualizada = new Posicao(produtoId, posicaoId, DateTime.Now, DateTime.Now, 100, false);


            // ACT
            await posicaoService.Atualizar(posicaoAtualizada);

            // Assert
            mocker.GetMock<IPosicaoRepository>().Verify(r => r.Atualizar(posicaoAtualizada), Times.Once);
        }

        [Fact(DisplayName = "04 Atualizar Posição Inválida")]
        [Trait("Categoria", "Posicao")]
        public async void AtualizarPosicao_AtualizarPosicaoInalida_NaoDeveAtualizarPosicao()
        {
            // AAA
            var produtoId = Guid.NewGuid();
            var posicaoId = Guid.NewGuid();

            var posicao = new Posicao(produtoId, posicaoId, DateTime.Now, DateTime.Now, 0, true);

            var mocker = new AutoMocker();
            var posicaoService = mocker.CreateInstance<PosicaoService>();

            await posicaoService.Adicionar(posicao);

            var posicaoAtualizada = new Posicao(produtoId, posicaoId, DateTime.Now, DateTime.Now, 0m, false);

            
            // ACT
            await posicaoService.Atualizar(posicaoAtualizada);

            // Assert
            mocker.GetMock<IPosicaoRepository>().Verify(r => r.Atualizar(posicaoAtualizada), Times.Never);
        }

        [Fact(DisplayName = "05 Remover Posição Válida")]
        [Trait("Categoria", "Posicao")]
        public async void RemoverPosicao_RemoverPosicaoValida_DeveRemoverPosicao()
        {
            // AAA
            var posicaoId = Guid.NewGuid();

            var posicao = new Posicao(posicaoId, Guid.NewGuid(), DateTime.Now, DateTime.Now, 100.14m, true);

            var mocker = new AutoMocker();
            var posicaoService = mocker.CreateInstance<PosicaoService>();

            // ACT
            await posicaoService.Remover(posicaoId);

            // Assert
            mocker.GetMock<IPosicaoRepository>().Verify(r => r.Remover(posicaoId), Times.Once);
        }

        [Fact(DisplayName = "06 Remover Posição Inválida")]
        [Trait("Categoria", "Posicao")]
        public async void RemoverPosicao_RemoverPosicaoInvalida_NaoDeveRemoverPosicao()
        {
            // AAA
            var posicaoId = Guid.NewGuid();

            var posicao = new Posicao(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now, 100.14m, true);

            var mocker = new AutoMocker();
            var posicaoService = mocker.CreateInstance<PosicaoService>();

            // ACT
            await posicaoService.Remover(posicaoId);

            // Assert
            mocker.GetMock<IPosicaoRepository>().Verify(r => r.Remover(posicaoId), Times.Once);
        }

    }
}
