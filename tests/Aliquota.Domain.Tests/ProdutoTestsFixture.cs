using Xunit;
using Bogus;
using System;
using System.Linq;
using Aliquota.Domain.Models;
using System.Collections.Generic;
using Moq.AutoMock;
using Aliquota.Domain.Services;

namespace Aliquota.Domain.Tests
{
    [CollectionDefinition(nameof(ProdutoCollection))]
    public class ProdutoCollection : ICollectionFixture<ProdutoTestsFixture> { }

    public class ProdutoTestsFixture : IDisposable
    {
        public ProdutoService ProdutoService;
        public AutoMocker Mocker;

        public Produto GerarProdutoPorId
        {
            get
            {
                var produto = new Faker<Produto>("pt_BR")
                .CustomInstantiator(f => new Produto(
                    Guid.Parse("7ABC988A-2A69-41D6-89A6-93A6B478C500"),
                    f.Company.CompanyName(),
                    f.Random.Number(0, 10),
                    true,
                    f.Date.Past(5, DateTime.Now.AddYears(-1))));

                return produto;
            }
        }

        public ProdutoService ObterProdutoService()
        {
            Mocker = new AutoMocker();
            ProdutoService = Mocker.CreateInstance<ProdutoService>();

            return ProdutoService;
        }

        public Produto GerarProdutoValido()
        {
            return GerarProdutos(1, true).FirstOrDefault();
        }

        public IEnumerable<Produto> ObterProdutosVariados()
        {
            var produtos = new List<Produto>();

            produtos.AddRange(GerarProdutos(50, true).ToList());
            produtos.AddRange(GerarProdutos(50, false).ToList());

            return produtos;
        }

        public IEnumerable<Produto> GerarProdutos(int quantidade, bool ativo)
        {
            var produtos = new Faker<Produto>("pt_BR")
                .CustomInstantiator(f => new Produto(
                    Guid.NewGuid(),
                    f.Company.CompanyName(),
                    f.Random.Number(0, 12),
                    ativo,
                    f.Date.Past(5, DateTime.Now.AddYears(-1))));

            return produtos.Generate(quantidade);
        }

        public Produto GerarProdutoInvalido()
        {
            var produto = new Faker<Produto>("pt_BR")
                .CustomInstantiator(f => new Produto(
                    Guid.NewGuid(),
                    f.Lorem.Letter(),
                    f.Random.Number(0, 12),
                    true,
                    f.Date.Past(5, DateTime.Now.AddYears(-1))));

            return produto;
        }

        public void Dispose() { }
    }
}
