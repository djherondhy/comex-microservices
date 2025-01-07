
using StockService.Models;

namespace StockService.Repository
{
    public interface IProductRepository
    {
        Task SaveChangesAsync();
        Task<Produto> GetProductById(int id);
    }
}
