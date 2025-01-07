

using StockService.Data.Dtos;

namespace StockService.ItemServiceHttpClient
{
    public interface IOrderServiceHttpClient
    {
        void SendProductToOrderService(ReadProductDto readProductDto);
    }
}
