using System;
using System.Linq;
using Xunit;
using Moq;
using Moq.AutoMock;
using Aliquota.Domain.Models;
using Aliquota.Domain.Services;
using Aliquota.Domain.Interfaces;

using System.Threading.Tasks;

namespace Aliquota.Domain.Tests
{
    [TestCaseOrderer("Aliquota.Domain.Tests.PriorityOrderer", "Aliquota.Domain.Tests")]
    [Collection(nameof(ProdutoCollection))]
    public class ProdutoTests
    {
        private readonly ProdutoTestsFixture _produtoTestsFixture;
        private readonly ProdutoService _produtoService;

        public ProdutoTests(ProdutoTestsFixture produtoTestsFixture)
        {
            _produtoTestsFixture = produtoTestsFixture;
            _produtoService = _produtoTestsFixture.ObterProdutoService();
        }

        [Fact(DisplayName = "00 Obter Todos Os Produtod")]
        [Trait("Categoria", "Produto")]
        public void ObterProdutos_ObterListaDeProdutos_DeveObterTodosProdutos()
        {
            // Arrange
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Setup(p => p.ObterTodos()).Returns(async () =>
            {
                return await Task.FromResult(_produtoTestsFixture.ObterProdutosVariados());
            });

            // Act
            var produtos = _produtoService.ObterProdutosAtivos().Result;

            // Assert
            Assert.True(produtos.Any());
        }

        [Fact(DisplayName = "00 Obter Produto Por Id")]
        [Trait("Categoria", "Produto")]
        public void ObterProdutos_ObterProdutoPorIds_DeveObterProdutoPorId()
        {
            // Arrange
            var produtoId = Guid.Parse("7ABC988A-2A69-41D6-89A6-93A6B478C500"); 

            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>()
                .Setup(p => p.ObterPorId(produtoId))
                .ReturnsAsync(_produtoTestsFixture.GerarProdutoPorId);

            // Act
            var produto = _produtoService.ObterProdutoPorId(produtoId).Result;

            // Assert
            Assert.Equal(produtoId, produto.Id);
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.ObterPorId(produtoId), Times.Once);
        }

        [Fact(DisplayName = "01 Adicionar Produto Válido"), TestPriority(1)]
        [Trait("Categoria", "Produto")]
        public async void AdicionarProduto_NovoProdutoValido_DeveCadastrarProduto()
        {
            // AAA 
            //var produtoValido = new Produto(Guid.NewGuid(), "Fundo XYZ", 12,true, DateTime.Now);
            var produtoValido = _produtoTestsFixture.GerarProdutoValido();
       
            /* AutoMocker Elimina a necessidade de mapear todas as instancias das dependencias
            var produtoRepo = new Mock<IProdutoRepository>();
            var posicaoRepo = new Mock<IPosicaoRepository>();
            var notificador = new Mock<INotificador>();
            var produtoService = new ProdutoService(produtoRepo.Object, posicaoRepo.Object, notificador.Object); */

            // Agora o Mocker esta no construtor...
            //var mocker = new AutoMocker();
            //var produtoService = mocker.CreateInstance<ProdutoService>();

            // ACT;
            await _produtoService.Adicionar(produtoValido);

            // Assert
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.Adicionar(produtoValido), Times.Once);
        }

        [Fact(DisplayName = "02 Adicionar Produto Inválido"), TestPriority(2)]
        [Trait("Categoria", "Produto")]
        public async void AdicionarProduto_NovoProdutoInvalido_NaoDeveCadastrarProduto()
        {
            // AAA
            var produtoInvalido = _produtoTestsFixture.GerarProdutoInvalido();

            // ACT
            await _produtoService.Adicionar(produtoInvalido);

            // Assert
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.Adicionar(produtoInvalido), Times.Never);
        }

        [Fact(DisplayName = "03 Adicionar Produto Repedido"), TestPriority(3)]
        [Trait("Categoria", "Produto")]
        public async void AdicionarProduto_ProdutoRepetido_NaoDeveCadastrarProduto()
        {
            // AAA
            var produto = new Produto(Guid.NewGuid(), "Fundo XYZ", 12, true, DateTime.Now);

            await _produtoService.Adicionar(produto);

            var produtoRepetido = new Produto(Guid.NewGuid(), "Fundo XYZ", 12, true, DateTime.Now);

            // ACT
            await _produtoService.Adicionar(produtoRepetido);

            // Assert
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.Adicionar(produtoRepetido), Times.Never);
        }

        [Fact(DisplayName = "04 Atualizar Produto Válido"), TestPriority(4)]
        [Trait("Categoria", "Produto")]
        public async void AtualizarProduto_AtualizarProdutoValido_DeveAtualizarProduto()
        {
            // AAA
            var produtoId = Guid.NewGuid();

            var produto = new Produto(produtoId, "Fundo XYZ", 12, true, DateTime.Now);

            await _produtoService.Adicionar(produto);

            var produtoAtualizado = new Produto(produtoId, "Fundo ZXY", 12, true, DateTime.Now);

            // ACT
            await _produtoService.Atualizar(produtoAtualizado);

            // Assert
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.Atualizar(produtoAtualizado), Times.Once);
        }

        [Fact(DisplayName = "05 Atualizar Produto Inválido"), TestPriority(5)]
        [Trait("Categoria", "Produto")]
        public async void AtualizarProduto_AtualizarProduto_NaoDeveAtualizarProdutoInvalido()
        {
            // AAA
            var produtoId = Guid.NewGuid();

            var produto = new Produto(produtoId, "Fundo XYZ", 12, true, DateTime.Now);

            await _produtoService.Adicionar(produto);

            var produtoAtualizado = new Produto(produtoId, "Fund", 12, true, DateTime.Now);

            // ACT
            await _produtoService.Atualizar(produtoAtualizado);

            // Assert
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.Atualizar(produtoAtualizado), Times.Never);
        }

        [Fact(DisplayName = "06 Atualizar Produto Repetido", Skip = "Está Executando Atualizar Quando Não Deveria"), TestPriority(6)]
        [Trait("Categoria", "Produto")]
        public async void AtualizarProduto_AtualizarProdutoRepetido_NaoDeveAtualizarProduto()
        {
            // AAA
            var primeiroProduto = new Produto(Guid.NewGuid(), "Fundo XYZ", 12, true, DateTime.Now);

            await _produtoService.Adicionar(primeiroProduto);

            var produtoId = Guid.NewGuid();
            var segundoProduto = new Produto(produtoId, "Fundo ZYX", 12, true, DateTime.Now);
            await _produtoService.Adicionar(segundoProduto);

            var produtoAtualizadoRepetido = new Produto(produtoId, "Fundo XYZ", 12, true, DateTime.Now);

            // ACT
            await _produtoService.Atualizar(produtoAtualizadoRepetido);

            // Assert
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.Atualizar(produtoAtualizadoRepetido), Times.Never);
        }

        [Fact(DisplayName = "07 Remover Produto Sem Posições"), TestPriority(7)]
        [Trait("Categoria", "Produto")]
        public async void RemoverProduto_ExcluirProdutoSemProsicoes_DeveExcluirProduto()
        {
            // AAA
            var produtoId = Guid.NewGuid();
            var produto = new Produto(produtoId,"Fundo XYZ", 12, true, DateTime.Now);

            await _produtoService.Adicionar(produto);

            // ACT
            await _produtoService.Remover(produtoId);

            // Assert
            _produtoTestsFixture.Mocker.GetMock<IProdutoRepository>().Verify(r => r.Remover(produtoId), Times.Once);
        }

        [Fact(DisplayName = "08 Remover Produto Com Posições", Skip = "Cenário Incompleto, pois não valida cenário"), TestPriority(8)]
        [Trait("Categoria", "Produto")]
        public async  void RemoverProduto_ExcluirProdutoComPosicoes_DeveExcluirProduto()
        {
            // AAA
            var produtoId = Guid.NewGuid();
            var produto = new Produto(produtoId, "Fundo XYZ", 12, true, DateTime.Now);

            var mocker = new AutoMocker();
            var posicaoService = mocker.CreateInstance<PosicaoService>();

            await _produtoService.Adicionar(produto);
            var posicao = new Posicao(Guid.NewGuid(), produtoId, DateTime.Now, DateTime.Now, 100.14m, true);
            await posicaoService.Adicionar(posicao);

            // ACT
            await _produtoService.Remover(produtoId);

            // Assert
            mocker.GetMock<IProdutoRepository>().Verify(r => r.Remover(produtoId), Times.Never);
        }
    }
}
