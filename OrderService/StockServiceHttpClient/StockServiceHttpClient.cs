using OrderService.Data.Dtos.Product;


namespace OrderService.ItemServiceHttpClient
{
    public interface IStockServiceHttpClient
    {
        Task<List<ReadProductDto>> GetAllProducts(int skip = 0, int take = 50);
        Task<ReadProductDto> GetProductById(int id);
    }
}
