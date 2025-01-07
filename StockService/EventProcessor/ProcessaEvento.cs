using System.Text.Json;
using AutoMapper;
using StockService.Data.Dtos.RabbitMq;
using StockService.Repository;

namespace StockService.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public async void Processa(string mensagem)
        {
            using var scope = _scopeFactory.CreateScope();
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

            var updateProductQuantityInStockDtoList = JsonSerializer.Deserialize<IList<UpdateProductQuantityInStockDto>>(mensagem);

            foreach (var updateProductQuantityInStockDto in updateProductQuantityInStockDtoList)
            {
                var product = await productRepository.GetProductById(updateProductQuantityInStockDto.ProductId);

                if (product != null)
                {
                    product.AvailableQuantity -= updateProductQuantityInStockDto.Amount;

                    await productRepository.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("Produto não encontrado: " + updateProductQuantityInStockDto.ProductId);
                }
            }

        }
    }
}