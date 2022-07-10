using Aliquota.Domain.Models;

namespace Aliquota.Domain.Interfaces
{
    public interface IProdutoService : IDisposable
    {
        Task <IEnumerable<Produto>> ObterProdutosAtivos();
        Task<Produto> ObterProdutoPorId(Guid id);
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(Guid id);
    }
}
