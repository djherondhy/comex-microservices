using OrderService.Data.Dtos.RabbitMq;

namespace OrderService.RabbitMqClient
{
    public interface IRabbitMqClient
    {
        void UpdateProductQuantityInStock(List<UpdateProductQuantityInStockDto> updateProductQuantityInStockDto);
    }
}